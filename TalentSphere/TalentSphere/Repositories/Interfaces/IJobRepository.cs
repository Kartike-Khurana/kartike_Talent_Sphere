using TalentSphere.DTOs.Common;
using TalentSphere.DTOs.Job;
using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<Job> AddAsync(Job job);
        Task<Job?> GetByIdAsync(int id);
        Task<List<Job>> GetAllAsync();
        Task<PagedResult<Job>> GetPagedAsync(JobFilterParams filters);
        Task UpdateAsync(Job job);
        Task DeleteAsync(Job job);
        Task SaveChangesAsync();
    }
}
