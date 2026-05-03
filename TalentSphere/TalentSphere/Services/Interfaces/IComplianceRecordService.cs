using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IComplianceRecordService
    {
        Task<ComplianceRecordResponseDTO> CreateComplianceRecordAsync(CreateComplianceRecordDTO dto);
        Task<ComplianceRecordResponseDTO> UpdateComplianceRecordAsync(int id, UpdateComplianceRecordDTO dto);
        Task<ComplianceRecordResponseDTO> GetComplianceRecordByIdAsync(int id);
        Task<IEnumerable<ComplianceRecordResponseDTO>> GetAllComplianceRecordsAsync();
        Task<bool> DeleteComplianceRecordAsync(int id);
    }
}
