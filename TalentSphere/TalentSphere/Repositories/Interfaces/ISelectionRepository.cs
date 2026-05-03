using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface ISelectionRepository
    {
        Task<Selection> AddAsync(Selection selection);
        Task<Selection?> GetByIdAsync(int id);
        Task<Selection?> GetByApplicationIdAsync(int applicationId);
        Task<List<Selection>> GetAllAsync();
        Task<List<Selection>> GetAllWithDetailsAsync();
        Task UpdateAsync(Selection selection);
        Task DeleteAsync(Selection selection);
        Task SaveChangesAsync();
    }
}
