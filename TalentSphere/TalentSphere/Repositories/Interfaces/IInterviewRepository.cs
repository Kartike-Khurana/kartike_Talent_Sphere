using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IInterviewRepository
    {
        Task<Models.Interview> AddAsync(Models.Interview interview);
        Task<Models.Interview?> GetByIdAsync(int id);
        Task<Models.Interview?> GetByIdWithDetailsAsync(int id);
        Task<List<Models.Interview>> GetAllAsync();
        Task<List<Models.Interview>> GetAllWithDetailsAsync();
        Task<List<Models.Interview>> GetByApplicationIdAsync(int applicationId);
        Task UpdateAsync(Models.Interview interview);
        Task DeleteAsync(Models.Interview interview);
        Task SaveChangesAsync();
    }
}
