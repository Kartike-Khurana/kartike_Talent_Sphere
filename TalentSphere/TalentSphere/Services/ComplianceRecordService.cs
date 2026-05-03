using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Notification;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class ComplianceRecordService : IComplianceRecordService
    {
        private readonly IComplianceRecordRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public ComplianceRecordService(
            IComplianceRecordRepository repository,
            IEmployeeRepository employeeRepository,
            INotificationService notificationService,
            IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<ComplianceRecordResponseDTO> CreateComplianceRecordAsync(CreateComplianceRecordDTO dto)
        {
            var record = _mapper.Map<ComplianceRecord>(dto);
            var saved = await _repository.AddComplianceRecordAsync(record);

            // Notify the employee about the new compliance requirement
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeID);
            if (employee != null && employee.UserId > 0)
            {
                await _notificationService.CreateNotificationAsync(new CreateNotificationDTO
                {
                    UserID = employee.UserId,
                    EntityID = saved.ComplianceID,
                    Message = !string.IsNullOrWhiteSpace(dto.Description)
                        ? $"Compliance ({dto.RecordType}): {dto.Description}"
                        : $"A new {dto.RecordType} compliance requirement has been added for you.",
                    Category = NotificationCategory.Compliance,
                });
            }

            // Reload with Employee navigation for EmployeeName
            var reloaded = await _repository.GetComplianceRecordByIdAsync(saved.ComplianceID);
            return _mapper.Map<ComplianceRecordResponseDTO>(reloaded ?? saved);
        }

        public async Task<ComplianceRecordResponseDTO> UpdateComplianceRecordAsync(int id, UpdateComplianceRecordDTO dto)
        {
            var record = await _repository.GetComplianceRecordByIdAsync(id);
            if (record == null) return null;
            _mapper.Map(dto, record);
            await _repository.UpdateComplianceRecordAsync(record);
            var reloaded = await _repository.GetComplianceRecordByIdAsync(id);
            return _mapper.Map<ComplianceRecordResponseDTO>(reloaded ?? record);
        }

        public async Task<ComplianceRecordResponseDTO> GetComplianceRecordByIdAsync(int id)
        {
            var record = await _repository.GetComplianceRecordByIdAsync(id);
            return record == null ? null : _mapper.Map<ComplianceRecordResponseDTO>(record);
        }

        public async Task<IEnumerable<ComplianceRecordResponseDTO>> GetAllComplianceRecordsAsync()
        {
            var records = await _repository.GetAllComplianceRecordsAsync();
            return records.Select(r => _mapper.Map<ComplianceRecordResponseDTO>(r));
        }

        public async Task<bool> DeleteComplianceRecordAsync(int id)
        {
            return await _repository.DeleteComplianceRecordAsync(id);
        }

        private static string GetComplianceAction(string recordType) => recordType?.ToLower() switch
        {
            "training" => "Please complete the required training.",
            "background" => "A background check has been initiated.",
            _ => "Please upload the required document."
        };
    }
}
