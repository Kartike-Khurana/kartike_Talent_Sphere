using System.Threading.Tasks;
using System.Collections.Generic;
using TalentSphere.DTOs;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces
{
    public interface IAuditLogService
    {
        // Audit log creation via public service removed.
        Task<AuditLogResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<AuditLogResponseDto>> GetAllAsync();
        Task<bool> DeleteAuditLogAsync(int id);
    }
}
