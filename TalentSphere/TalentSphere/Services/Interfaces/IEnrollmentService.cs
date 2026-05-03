using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDTO> CreateEnrollmentAsync(CreateEnrollmentDTO dto);
        Task<EnrollmentResponseDTO?> GetByIdAsync(int id);
        Task<List<EnrollmentResponseDTO>> GetAllAsync();
        Task<List<EnrollmentResponseDTO>> GetByEmployeeIdAsync(int employeeId);
        Task<EnrollmentResponseDTO?> StartEnrollmentAsync(int id);
        Task<EnrollmentResponseDTO?> CompleteEnrollmentAsync(int id, CompleteEnrollmentDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
