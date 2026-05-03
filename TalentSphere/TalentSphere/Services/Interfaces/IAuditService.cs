using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IAuditService
    {
        Task<AuditResponseDTO> CreateAuditAsync(CreateAuditDTO dto, int hrUserId);
        Task<AuditResponseDTO> UpdateAuditAsync(int id, UpdateAuditDTO dto);
        Task<AuditResponseDTO> GetAuditByIdAsync(int id);
        Task<IEnumerable<AuditResponseDTO>> GetAllAuditsAsync();
        Task<bool> DeleteAuditAsync(int id);
    }
}
