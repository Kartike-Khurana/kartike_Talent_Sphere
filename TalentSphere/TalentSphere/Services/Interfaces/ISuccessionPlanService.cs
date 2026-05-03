using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface ISuccessionPlanService
    {
        Task<SuccessionPlanResponseDTO> CreateSuccessionPlanAsync(CreateSuccessionPlanDTO dto);
        Task<SuccessionPlanResponseDTO?> GetByIdAsync(int id);
        Task<List<SuccessionPlanResponseDTO>> GetAllAsync();
        Task<SuccessionPlanResponseDTO?> UpdateAsync(int id, UpdateSuccesionPlanDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
