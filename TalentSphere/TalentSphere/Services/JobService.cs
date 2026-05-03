using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Common;
using TalentSphere.DTOs.Job;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<JobService> _logger;

        public JobService(IJobRepository repository, IMapper mapper, ILogger<JobService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Job> CreateJobAsync(CreateJobDTO dto)
        {
            var job = _mapper.Map<Job>(dto);
            job.PostedDate = DateTime.UtcNow;
            job.CreatedAt = DateTime.UtcNow;
            job.Status = JobStatus.Open;
            var added = await _repository.AddAsync(job);
            await _repository.SaveChangesAsync();
            _logger.LogInformation("Job '{Title}' created with ID {JobID}", job.Title, job.JobID);
            return added;
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Job>> GetAllJobsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PagedResult<Job>> GetPagedJobsAsync(JobFilterParams filters)
        {
            if (filters.PageSize > 100) filters.PageSize = 100;
            if (filters.Page < 1) filters.Page = 1;
            return await _repository.GetPagedAsync(filters);
        }

        public async Task<Job?> UpdateJobAsync(int id, UpdateJobDTO dto)
        {
            var job = await _repository.GetByIdAsync(id);
            if (job == null) return null;
            _mapper.Map(dto, job);
            job.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(job);
            await _repository.SaveChangesAsync();
            return job;
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            var job = await _repository.GetByIdAsync(id);
            if (job == null) return false;
            await _repository.DeleteAsync(job);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
