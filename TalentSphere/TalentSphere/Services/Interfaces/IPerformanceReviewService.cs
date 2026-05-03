using TalentSphere.DTOs.PerformanceReview;

namespace TalentSphere.Services.Interfaces
{
    public interface IPerformanceReviewService
    {
        Task<PerformanceReviewDTO> CreateReviewAsync(CreatePerformanceReviewDTO dto, int managerId);
        Task<PerformanceReviewDTO?> GetByIdAsync(int id);
        Task<List<PerformanceReviewListDTO>> GetAllReviewsAsync(int? employeeId = null);
        Task<PerformanceReviewDTO?> UpdateReviewAsync(int id, UpdatePerformanceReviewDTO dto);
        Task<bool> DeleteReviewAsync(int id);
    }
}
