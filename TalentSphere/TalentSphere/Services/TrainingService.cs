using TalentSphere.DTOs;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _repository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public TrainingService(ITrainingRepository repository, IEnrollmentRepository enrollmentRepository)
        {
            _repository = repository;
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<TrainingResponseDTO> CreateTrainingAsync(CreateTrainingDTO dto)
        {
            var training = new Training
            {
                Title = dto.Title,
                Description = dto.Description,
                TrainingType = ParseTrainingType(dto.TrainingType),
                DeliveryMode = ParseDeliveryMode(dto.DeliveryMode),
                TrainingLink = dto.TrainingLink,
                Location = dto.Location,
                InstructorName = dto.InstructorName,
                ClassStartTime = dto.ClassStartTime,
                ClassEndTime = dto.ClassEndTime,
                MaxCapacity = dto.MaxCapacity,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                status = ParseTrainingStatus(dto.Status),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var added = await _repository.AddAsync(training);
            await _repository.SaveChangesAsync();
            return MapToResponse(added);
        }

        public async Task<TrainingResponseDTO?> GetByIdAsync(int id)
        {
            var training = await _repository.GetByIdAsync(id);
            return training is null ? null : MapToResponse(training);
        }

        public async Task<List<TrainingResponseDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return list.Select(MapToResponse).ToList();
        }

        public async Task<TrainingResponseDTO?> UpdateAsync(int id, UpdateTrainingDTO dto)
        {
            var training = await _repository.GetByIdAsync(id);
            if (training is null) return null;

            if (dto.Title != null) training.Title = dto.Title;
            if (dto.Description != null) training.Description = dto.Description;
            if (dto.TrainingType != null) training.TrainingType = ParseTrainingType(dto.TrainingType);
            if (dto.DeliveryMode != null) training.DeliveryMode = ParseDeliveryMode(dto.DeliveryMode);
            if (dto.TrainingLink != null) training.TrainingLink = dto.TrainingLink;
            if (dto.Location != null) training.Location = dto.Location;
            if (dto.InstructorName != null) training.InstructorName = dto.InstructorName;
            if (dto.ClassStartTime != null) training.ClassStartTime = dto.ClassStartTime;
            if (dto.ClassEndTime != null) training.ClassEndTime = dto.ClassEndTime;
            if (dto.MaxCapacity.HasValue) training.MaxCapacity = dto.MaxCapacity;
            if (dto.StartDate.HasValue) training.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue) training.EndDate = dto.EndDate.Value;
            if (dto.Status != null) training.status = ParseTrainingStatus(dto.Status);
            training.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return MapToResponse(training);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var training = await _repository.GetByIdAsync(id);
            if (training is null) return false;

            training.IsDeleted = true;
            training.UpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<TrainingStatsDTO> GetStatsAsync()
        {
            var trainings = await _repository.GetAllAsync();
            var enrollments = await _enrollmentRepository.GetAllAsync();
            var now = DateTime.UtcNow;

            var total = enrollments.Count;
            var completed = enrollments.Count(e => e.status == EnrollmentStatus.Completed);
            var overdue = enrollments.Count(e =>
                e.status != EnrollmentStatus.Completed &&
                e.status != EnrollmentStatus.Cancelled &&
                e.DueDate.HasValue && e.DueDate.Value < now);

            return new TrainingStatsDTO
            {
                TotalTrainings = trainings.Count,
                ActiveTrainings = trainings.Count(t => t.status == TrainingStatus.Active),
                TotalEnrollments = total,
                CompletedEnrollments = completed,
                OverdueEnrollments = overdue,
                CompletionRate = total > 0 ? Math.Round((double)completed / total * 100, 1) : 0
            };
        }

        private static TrainingResponseDTO MapToResponse(Training t) => new()
        {
            TrainingID = t.TrainingID,
            Title = t.Title,
            Description = t.Description,
            TrainingType = t.TrainingType.ToString(),
            DeliveryMode = t.DeliveryMode.ToString(),
            TrainingLink = t.TrainingLink,
            Location = t.Location,
            InstructorName = t.InstructorName,
            ClassStartTime = t.ClassStartTime,
            ClassEndTime = t.ClassEndTime,
            MaxCapacity = t.MaxCapacity,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            DurationDays = (t.EndDate - t.StartDate).Days,
            Status = t.status.ToString(),
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        };

        private static TrainingType ParseTrainingType(string value) =>
            Enum.TryParse<TrainingType>(value, true, out var r) ? r : TrainingType.Other;

        private static DeliveryMode ParseDeliveryMode(string value) =>
            Enum.TryParse<DeliveryMode>(value, true, out var r) ? r : DeliveryMode.Online;

        private static TrainingStatus ParseTrainingStatus(string value) =>
            Enum.TryParse<TrainingStatus>(value, true, out var r) ? r : TrainingStatus.Planned;
    }
}
