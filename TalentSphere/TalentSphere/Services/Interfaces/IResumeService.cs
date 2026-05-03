using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeResponseDTO> UploadResumeAsync(int candidateId, IFormFile file);
        Task<ResumeResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ResumeResponseDTO>> GetAllAsync();
        Task<IEnumerable<ResumeResponseDTO>> GetByCandidateIdAsync(int candidateId);
        Task<ResumeResponseDTO?> ReplaceFileAsync(int id, IFormFile file);
        Task<bool> DeleteResumeAsync(int id);
    }
}
