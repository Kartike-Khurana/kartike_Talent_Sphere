using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Notification;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class EmployeeDocumentService : IEmployeeDocumentService
    {
        private readonly IEmployeeDocumentRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public EmployeeDocumentService(
            IEmployeeDocumentRepository repository,
            IEmployeeRepository employeeRepository,
            INotificationService notificationService,
            IWebHostEnvironment env,
            IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _notificationService = notificationService;
            _env = env;
            _mapper = mapper;
        }

        public async Task<EmployeeDocumentResponseDto> CreateEmployeeDocumentAsync(CreateEmployeeDocumentDTO dto)
        {
            var doc = _mapper.Map<EmployeeDocument>(dto);
            doc.CreatedAt = DateTime.UtcNow;
            doc.FileURI = string.Empty;

            var added = await _repository.AddAsync(doc);
            await _repository.SaveChangesAsync();
            return _mapper.Map<EmployeeDocumentResponseDto>(added);
        }

        public async Task<EmployeeDocumentResponseDto> GetByIdAsync(int id)
        {
            var doc = await _repository.GetByIdAsync(id);
            if (doc == null) return null;
            return _mapper.Map<EmployeeDocumentResponseDto>(doc);
        }

        public async Task<IEnumerable<EmployeeDocumentResponseDto>> GetAllAsync()
        {
            var docs = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDocumentResponseDto>>(docs);
        }

        public async Task<IEnumerable<EmployeeDocumentResponseDto>> GetByEmployeeIdAsync(int employeeId)
        {
            var docs = await _repository.GetByEmployeeIdAsync(employeeId);
            return _mapper.Map<IEnumerable<EmployeeDocumentResponseDto>>(docs);
        }

        public async Task<EmployeeDocumentResponseDto> UploadDocumentAsync(int employeeId, string docType, IFormFile file)
        {
            // Validate employee exists
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return null;

            // Save file to wwwroot/uploads/documents/{employeeId}/
            var uploadFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                "uploads", "documents", employeeId.ToString());
            Directory.CreateDirectory(uploadFolder);

            var ext = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            var fileUri = $"/uploads/documents/{employeeId}/{fileName}";

            if (!Enum.TryParse<EmployeeDocType>(docType, true, out var parsedDocType))
                parsedDocType = EmployeeDocType.Resume;

            var doc = new EmployeeDocument
            {
                EmployeeID = employeeId,
                DocType = parsedDocType,
                FileURI = fileUri,
                UploadedDate = DateTime.UtcNow,
                VerifyStatus = EmployeeDocVerifyStatus.Pending,
                CreatedAt = DateTime.UtcNow,
            };

            var added = await _repository.AddAsync(doc);
            await _repository.SaveChangesAsync();

            // Notify the employee
            if (employee.UserId > 0)
            {
                await _notificationService.CreateNotificationAsync(new CreateNotificationDTO
                {
                    UserID = employee.UserId,
                    EntityID = added.DocumentID,
                    Message = $"Your {parsedDocType} document has been uploaded successfully and is pending verification.",
                    Category = NotificationCategory.System,
                });
            }

            return _mapper.Map<EmployeeDocumentResponseDto>(added);
        }

        public async Task<bool> SendDocumentReminderAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) return false;

            await _notificationService.CreateNotificationAsync(new CreateNotificationDTO
            {
                UserID = employee.UserId,
                EntityID = employeeId,
                Message = "Action required: Please upload your required documents to complete your employee profile.",
                Category = NotificationCategory.System,
            });

            return true;
        }

        public async Task<EmployeeDocumentResponseDto> UpdateEmployeeDocumentAsync(int id, UpdateEmployeeDocumentDTO dto)
        {
            var doc = await _repository.GetByIdAsync(id);
            if (doc == null) return null;

            if (dto.EmployeeID.HasValue) doc.EmployeeID = dto.EmployeeID.Value;
            if (!string.IsNullOrWhiteSpace(dto.FileURI)) doc.FileURI = dto.FileURI;
            if (dto.UploadedDate.HasValue) doc.UploadedDate = dto.UploadedDate.Value;
            if (dto.DocType.HasValue) doc.DocType = dto.DocType.Value;
            if (dto.VerifyStatus.HasValue) doc.VerifyStatus = dto.VerifyStatus.Value;

            doc.UpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return _mapper.Map<EmployeeDocumentResponseDto>(doc);
        }

        public async Task<bool> DeleteEmployeeDocumentAsync(int id)
        {
            var doc = await _repository.GetByIdAsync(id);
            if (doc == null) return false;

            doc.IsDeleted = true;
            doc.UpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
