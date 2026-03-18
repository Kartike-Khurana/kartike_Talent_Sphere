using System.Threading.Tasks;
using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IAuditLogRepository
    {
        // Audit log creation via repository is removed; audit logs are written internally, not via public API.
        Task<AuditLog> GetByIdAsync(int id);
        Task<IEnumerable<AuditLog>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
