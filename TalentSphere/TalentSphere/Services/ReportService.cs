using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Analytics;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository repository, AppDbContext context, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Report> CreateReportAsync(CreateReportDTO dto)
        {
            var report = _mapper.Map<Report>(dto);
            report.CreatedAt = DateTime.UtcNow;

            var added = await _repository.AddAsync(report);
            await _repository.SaveChangesAsync();
            return added;
        }

        public async Task<Report?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Report>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Report?> UpdateAsync(int id, UpdateReportDTO dto)
        {
            var report = await _repository.GetByIdAsync(id);
            if (report is null) return null;

            report.Scope = dto.Scope;
            report.Metrics = dto.Metrics;
            report.GenerateDate = dto.GenerateDate;
            report.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return report;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var report = await _repository.GetByIdAsync(id);
            if (report is null) return false;

            await _repository.DeleteAsync(report);
            await _repository.SaveChangesAsync();
            return true;
        }

        // ── Analytics ─────────────────────────────────────────────────────────

        public async Task<HiringAnalyticsDTO> GetHiringAnalyticsAsync()
        {
            var applicationsPerJob = await _context.Applications
                .Where(a => !a.IsDeleted)
                .GroupBy(a => new { a.JobID, a.Job.Title, a.Job.Department })
                .Select(g => new JobApplicationCountDTO
                {
                    JobID = g.Key.JobID,
                    JobTitle = g.Key.Title,
                    Department = g.Key.Department,
                    ApplicationCount = g.Count(),
                    HiredCount = g.Count(a => a.Status == ApplicationStatus.Accepted)
                })
                .OrderByDescending(x => x.ApplicationCount)
                .ToListAsync();

            var totalApps = applicationsPerJob.Sum(x => x.ApplicationCount);
            var totalJobs = applicationsPerJob.Count;

            return new HiringAnalyticsDTO
            {
                TotalJobs = totalJobs,
                TotalApplications = totalApps,
                TotalHired = applicationsPerJob.Sum(x => x.HiredCount),
                AverageApplicationsPerJob = totalJobs == 0 ? 0 : Math.Round((double)totalApps / totalJobs, 2),
                ApplicationsPerJob = applicationsPerJob
            };
        }

        public async Task<PerformanceAnalyticsDTO> GetPerformanceAnalyticsAsync()
        {
            var reviews = await _context.PerformanceReviews
                .Include(r => r.Employee)
                .Where(r => !r.IsDeleted)
                .ToListAsync();

            var byDepartment = reviews
                .GroupBy(r => r.Employee?.Department ?? "Unknown")
                .Select(g => new DepartmentPerformanceDTO
                {
                    Department = g.Key,
                    AverageScore = Math.Round((double)g.Average(r => r.Score), 2),
                    ReviewCount = g.Count()
                })
                .OrderByDescending(d => d.AverageScore)
                .ToList();

            var topPerformers = reviews
                .GroupBy(r => new { r.EmployeeID, Name = r.Employee?.Name ?? "Unknown", Dept = r.Employee?.Department ?? "Unknown" })
                .Select(g => new TopPerformerDTO
                {
                    EmployeeID = g.Key.EmployeeID,
                    EmployeeName = g.Key.Name,
                    Department = g.Key.Dept,
                    AverageScore = Math.Round((double)g.Average(r => r.Score), 2),
                    ReviewCount = g.Count()
                })
                .OrderByDescending(t => t.AverageScore)
                .Take(10)
                .ToList();

            return new PerformanceAnalyticsDTO
            {
                TotalReviews = reviews.Count,
                OverallAverageScore = reviews.Count == 0 ? 0 : Math.Round((double)reviews.Average(r => r.Score), 2),
                ByDepartment = byDepartment,
                TopPerformers = topPerformers
            };
        }

        public async Task<TrainingAnalyticsDTO> GetTrainingAnalyticsAsync()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Training)
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            var trainings = await _context.Trainings
                .Where(t => !t.IsDeleted)
                .ToListAsync();

            var byTraining = trainings.Select(t =>
            {
                var tEnrollments = enrollments.Where(e => e.TrainingID == t.TrainingID).ToList();
                var completed = tEnrollments.Count(e => e.status == EnrollmentStatus.Completed);
                var total = tEnrollments.Count;
                return new TrainingCompletionDTO
                {
                    TrainingID = t.TrainingID,
                    Title = t.Title,
                    Status = t.status.ToString(),
                    EnrollmentCount = total,
                    CompletedCount = completed,
                    CompletionRate = total == 0 ? 0 : Math.Round((double)completed / total * 100, 1)
                };
            })
            .OrderByDescending(t => t.EnrollmentCount)
            .ToList();

            var totalEnrollments = enrollments.Count;
            var totalCompleted = enrollments.Count(e => e.status == EnrollmentStatus.Completed);

            return new TrainingAnalyticsDTO
            {
                TotalTrainings = trainings.Count,
                TotalEnrollments = totalEnrollments,
                CompletedEnrollments = totalCompleted,
                OverallCompletionRate = totalEnrollments == 0 ? 0 : Math.Round((double)totalCompleted / totalEnrollments * 100, 1),
                ByTraining = byTraining
            };
        }
    }
}
