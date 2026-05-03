using TalentSphere.DTOs;
using TalentSphere.DTOs.Common;
using TalentSphere.DTOs.Job;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces
{
    public interface IJobService
    {
        Task<Job> CreateJobAsync(CreateJobDTO dto);
        Task<Job?> GetByIdAsync(int id);
        Task<List<Job>> GetAllJobsAsync();
        Task<PagedResult<Job>> GetPagedJobsAsync(JobFilterParams filters);
        Task<Job?> UpdateJobAsync(int id, UpdateJobDTO dto);
        Task<bool> DeleteJobAsync(int id);
    }
}
