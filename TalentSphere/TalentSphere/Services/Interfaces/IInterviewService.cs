using TalentSphere.DTOs;
using TalentSphere.DTOs.Interview;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces
{
    public interface IInterviewService
    {
        // Basic CRUD
        Task<Interview> CreateInterviewAsync(CreateInterviewDTO dto);
        Task<Interview?> GetByIdAsync(int id);
        Task<List<InterviewResponseDTO>> GetAllInterviewsAsync();
        Task<Interview?> UpdateInterviewAsync(int id, UpdateInterviewDTO dto);
        Task<bool> DeleteInterviewAsync(int id);

        // Workflow operations
        Task<InterviewResponseDTO> ScheduleInterviewAsync(ScheduleInterviewDTO dto);
        Task<InterviewResponseDTO> UpdateInterviewStatusAsync(int id, UpdateInterviewStatusDTO dto);
        Task<List<InterviewResponseDTO>> GetByApplicationIdAsync(int applicationId);
        Task<List<InterviewResponseDTO>> GetAllWithDetailsAsync();
        Task<InterviewResponseDTO?> GetDetailedByIdAsync(int id);
    }
}
