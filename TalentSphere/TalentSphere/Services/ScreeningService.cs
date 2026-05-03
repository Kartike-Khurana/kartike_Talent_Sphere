using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Notification;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class ScreeningService : IScreeningService
    {
        private readonly IScreeningRepository _repository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<ScreeningService> _logger;

        public ScreeningService(
            IScreeningRepository repository,
            IApplicationRepository applicationRepository,
            INotificationService notificationService,
            IMapper mapper,
            ILogger<ScreeningService> logger)
        {
            _repository = repository;
            _applicationRepository = applicationRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ScreeningResponseDTO> CreateScreeningAsync(CreateScreeningDTO dto)
        {
            var application = await _applicationRepository.GetByIdWithDetailsAsync(dto.ApplicationID)
                ?? throw new KeyNotFoundException($"Application {dto.ApplicationID} not found.");

            if (application.Status == ApplicationStatus.Rejected || application.Status == ApplicationStatus.Accepted)
                throw new InvalidOperationException(
                    $"Cannot screen an application with status '{application.Status}'.");

            var screening = _mapper.Map<Screening>(dto);
            screening.Date = DateTime.UtcNow;
            screening.CreatedAt = DateTime.UtcNow;
            screening.IsDeleted = false;

            var added = await _repository.AddAsync(screening);

            // Advance application to Reviewed when it's still in an early stage
            if (application.Status == ApplicationStatus.Pending || application.Status == ApplicationStatus.Submitted)
            {
                application.Status = ApplicationStatus.Reviewed;
                await _applicationRepository.SaveChangesAsync();
            }

            await _repository.SaveChangesAsync();

            // Notify candidate — wrapped in try/catch so a notification failure never fails the main flow
            try
            {
                var resultLabel = dto.Result switch
                {
                    ScreeningResult.Pass => "passed",
                    ScreeningResult.Fail => "did not pass",
                    _ => "is under review"
                };

                await _notificationService.CreateNotificationAsync(new CreateNotificationDTO
                {
                    UserID = application.CandidateID,
                    EntityID = added.ScreeningID,
                    Message = $"Your application for '{application.Job?.Title}' has been screened — result: {resultLabel}.",
                    Category = NotificationCategory.Recruitment
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Screening notification failed for ApplicationID {Id}", dto.ApplicationID);
            }

            return _mapper.Map<ScreeningResponseDTO>(added);
        }

        public async Task<ScreeningResponseDTO?> GetByIdAsync(int id)
        {
            var screening = await _repository.GetByIdAsync(id);
            return screening is null ? null : _mapper.Map<ScreeningResponseDTO>(screening);
        }

        public async Task<ScreeningResponseDTO?> GetByApplicationIdAsync(int applicationId)
        {
            var screening = await _repository.GetByApplicationIdAsync(applicationId);
            return screening is null ? null : _mapper.Map<ScreeningResponseDTO>(screening);
        }

        public async Task<IEnumerable<ScreeningResponseDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ScreeningResponseDTO>>(list);
        }

        public async Task<ScreeningResponseDTO?> UpdateScreeningAsync(int id, UpdateScreeningDTO dto)
        {
            var screening = await _repository.GetByIdAsync(id);
            if (screening is null) return null;

            _mapper.Map(dto, screening);
            screening.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(screening);
            await _repository.SaveChangesAsync();
            return _mapper.Map<ScreeningResponseDTO>(screening);
        }

        public async Task<bool> DeleteScreeningAsync(int id)
        {
            var screening = await _repository.GetByIdAsync(id);
            if (screening is null) return false;

            screening.IsDeleted = true;
            screening.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(screening);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
