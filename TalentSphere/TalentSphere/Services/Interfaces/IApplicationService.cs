using TalentSphere.DTOs;
using TalentSphere.DTOs.Application;
using TalentSphere.DTOs.Common;

namespace TalentSphere.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<ApplicationResponseDTO?> CreateApplicationAsync(CreateApplicationDTO dto);
        Task<ApplicationResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationResponseDTO>> GetAllAsync();
        Task<PagedResult<ApplicationResponseDTO>> GetPagedAsync(ApplicationFilterParams filters);
        Task<IEnumerable<ApplicationResponseDTO>> GetByJobIdAsync(int jobId);
        Task<IEnumerable<ApplicationResponseDTO>> GetByCandidateIdAsync(int candidateId);
        Task<ApplicationResponseDTO?> UpdateApplicationAsync(int id, UpdateApplicationDTO dto);
        Task<bool> DeleteApplicationAsync(int id);
    }
}
