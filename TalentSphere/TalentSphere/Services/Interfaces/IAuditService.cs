using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IAuditService
<<<<<<< Updated upstream
{
    Task<CreateAuditDTO> CreateAuditAsync(CreateAuditDTO createAuditDTO);
        Task<UpdateAuditDTO> UpdateAuditAsync(int id, UpdateAuditDTO updateAuditDTO);
        Task<AuditResponseDTO> GetAuditByIdAsync(int id);
        Task<IEnumerable<AuditResponseDTO>> GetAllAuditsAsync();
        Task<bool> DeleteAuditAsync(int id);
}
=======
    {
        Task<AuditLog> GetByIdAsync(int id);
    }
>>>>>>> Stashed changes
}
