using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IAuditRepository
    {
        Task<Audit> AddAuditAsync(Audit audit);
    }
}