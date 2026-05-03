using TalentSphere.DTOs;
using TalentSphere.DTOs.Selection;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces
{
    public interface ISelectionService
    {
        // Basic CRUD
        Task<Selection> CreateSelectionAsync(CreateSelectionDTO dto);
        Task<Selection?> GetByIdAsync(int id);
        Task<List<Selection>> GetAllSelectionsAsync();
        Task<Selection?> UpdateSelectionAsync(int id, UpdateSelectionDTO dto);
        Task<bool> DeleteSelectionAsync(int id);

        // Workflow
        Task<SelectionResponseDTO> MakeDecisionAsync(MakeSelectionDecisionDTO dto);
        Task<List<SelectionResponseDTO>> GetAllWithDetailsAsync();
        Task<SelectionResponseDTO?> GetByApplicationIdAsync(int applicationId);
    }
}
