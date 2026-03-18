using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IAuditRepository
    {
        Task<Audit> AddAuditAsync(Audit audit);
        Task<Audit> GetAuditByIdAsync(int id);
        Task<IEnumerable<Audit>> GetAllAuditsAsync();
        Task UpdateAuditAsync(Audit audit);
        Task<bool> DeleteAuditAsync(int id);
    }
}