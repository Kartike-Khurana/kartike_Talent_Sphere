using AutoMapper;
using TalentSphere.DTOs.Notification;
using TalentSphere.DTOs.PerformanceReview;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class PerformanceReviewService : IPerformanceReviewService
    {
        private readonly IPerformanceReviewRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<PerformanceReviewService> _logger;

        public PerformanceReviewService(
            IPerformanceReviewRepository repository,
            IEmployeeRepository employeeRepository,
            INotificationService notificationService,
            IMapper mapper,
            ILogger<PerformanceReviewService> logger)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _notificationService = notificationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PerformanceReviewDTO> CreateReviewAsync(CreatePerformanceReviewDTO dto, int managerId)
        {
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeID)
                ?? throw new KeyNotFoundException($"Employee {dto.EmployeeID} not found.");

            var review = _mapper.Map<PerformanceReview>(dto);
            review.ManagerID = managerId;
            review.CreatedAt = DateTime.UtcNow;
            review.IsDeleted = false;

            var added = await _repository.AddAsync(review);
            await _repository.SaveChangesAsync();

            try
            {
                await _notificationService.CreateNotificationAsync(new CreateNotificationDTO
                {
                    UserID = employee.UserId,
                    EntityID = added.ReviewID,
                    Message = $"A new performance review has been submitted for you with a rating of {dto.Rating}/5.",
                    Category = NotificationCategory.Performance
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Performance review notification failed for EmployeeID {Id}", dto.EmployeeID);
            }

            return _mapper.Map<PerformanceReviewDTO>(added);
        }

        public async Task<PerformanceReviewDTO?> UpdateReviewAsync(int id, UpdatePerformanceReviewDTO dto)
        {
            if (dto.Rating.HasValue && (dto.Rating.Value < 1 || dto.Rating.Value > 5))
                throw new ArgumentException("Rating must be between 1 and 5.");

            var existingReview = await _repository.GetByIdAsync(id);
            if (existingReview is null) return null;

            _mapper.Map(dto, existingReview);
            existingReview.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existingReview);
            await _repository.SaveChangesAsync();

            return _mapper.Map<PerformanceReviewDTO>(existingReview);
        }

        public async Task<PerformanceReviewDTO?> GetByIdAsync(int id)
        {
            var review = await _repository.GetByIdAsync(id);
            return review is null ? null : _mapper.Map<PerformanceReviewDTO>(review);
        }

        public async Task<List<PerformanceReviewListDTO>> GetAllReviewsAsync(int? employeeId = null)
        {
            var reviews = await _repository.GetAllAsync(employeeId);
            return _mapper.Map<List<PerformanceReviewListDTO>>(reviews);
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _repository.GetByIdAsync(id);
            if (review is null) return false;

            review.IsDeleted = true;
            review.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(review);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
