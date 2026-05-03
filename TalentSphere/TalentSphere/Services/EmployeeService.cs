using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Notification;
using TalentSphere.Models;
using TalentSphere.Enums;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            IEmployeeRepository repository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            INotificationService notificationService,
            IMapper mapper,
            ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EmployeeResponseDto> CreateEmployeeAsync(CreateEmployeeDTO dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            employee.CreatedAt = DateTime.UtcNow;
            if (employee.Status == 0)
                employee.Status = EmployeeStatus.Active;

            var added = await _repository.AddAsync(employee);
            await _repository.SaveChangesAsync();

            // Promote the user's role to Employee
            await PromoteToEmployeeRoleAsync(dto.UserId);

            // Notify the new employee to upload their documents
            await _notificationService.CreateNotificationAsync(new CreateNotificationDTO
            {
                UserID = dto.UserId,
                EntityID = added.EmployeeID,
                Message = "Welcome! Please upload your required documents (Resume, ID, Certificates) to complete your employee profile.",
                Category = NotificationCategory.System,
            });

            return _mapper.Map<EmployeeResponseDto>(added);
        }

        private async Task PromoteToEmployeeRoleAsync(int userId)
        {
            try
            {
                var employeeRole = await _roleRepository.GetByNameAsync(RoleName.Employee.ToString());
                if (employeeRole == null)
                {
                    _logger.LogWarning("Employee role not found — cannot promote user {UserId}", userId);
                    return;
                }

                var userRole = await _userRoleRepository.GetAnyByUserIdAsync(userId);
                if (userRole == null)
                {
                    _logger.LogWarning("No user role record found for user {UserId}", userId);
                    return;
                }

                userRole.RoleId = employeeRole.RoleID;
                userRole.IsDeleted = false;
                userRole.UpdatedAt = DateTime.UtcNow;
                await _userRoleRepository.UpdateAsync(userRole);
                await _userRoleRepository.SaveChangesAsync();

                _logger.LogInformation("User {UserId} promoted to Employee role", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to promote user {UserId} to Employee role", userId);
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetByUserIdAsync(int userId)
        {
            var employees = await _repository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        }


        public async Task<EmployeeResponseDto> UpdateEmployeeAsync(int id, UpdateEmployeeDTO dto)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) return null;

            // Apply partial update via AutoMapper mapping profile (ignore null/whitespace)
            _mapper.Map(dto, employee);
            employee.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null) return false;

            employee.IsDeleted = true;
            employee.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
