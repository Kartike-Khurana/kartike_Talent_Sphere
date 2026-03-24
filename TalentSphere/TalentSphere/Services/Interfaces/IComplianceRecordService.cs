using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IComplianceRecordService
    {
        Task<CreateComplianceRecordDTO> CreateComplianceRecordAsync(CreateComplianceRecordDTO createComplianceRecordDTO);
        Task<UpdateComplianceRecordDTO> UpdateComplianceRecordAsync(int id, UpdateComplianceRecordDTO updateComplianceRecordDTO);
        Task<ComplianceRecordResponseDTO> GetComplianceRecordByIdAsync(int id);
        Task<IEnumerable<ComplianceRecordResponseDTO>> GetAllComplianceRecordsAsync();
        Task<bool> DeleteComplianceRecordAsync(int id);
    }
}