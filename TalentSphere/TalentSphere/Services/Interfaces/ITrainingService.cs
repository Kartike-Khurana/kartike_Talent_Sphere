using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface ITrainingService
    {
        Task<TrainingResponseDTO> CreateTrainingAsync(CreateTrainingDTO dto);
        Task<TrainingResponseDTO?> GetByIdAsync(int id);
        Task<List<TrainingResponseDTO>> GetAllAsync();
        Task<TrainingResponseDTO?> UpdateAsync(int id, UpdateTrainingDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<TrainingStatsDTO> GetStatsAsync();
    }
}
