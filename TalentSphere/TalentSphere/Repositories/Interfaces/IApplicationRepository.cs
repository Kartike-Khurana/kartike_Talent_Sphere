using TalentSphere.DTOs.Application;
using TalentSphere.DTOs.Common;
using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application> AddAsync(Application application);
        Task<Application?> GetByIdAsync(int id);
        Task<Application?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Application>> GetAllAsync();
        Task<PagedResult<Application>> GetPagedAsync(ApplicationFilterParams filters);
        Task<IEnumerable<Application>> GetByJobIdAsync(int jobId);
        Task<IEnumerable<Application>> GetByCandidateIdAsync(int candidateId);
        Task SaveChangesAsync();
    }
}
