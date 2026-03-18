using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IComplianceRecordService
    {
        Task<CreateComplianceRecordDTO> CreateComplianceRecordAsync(CreateComplianceRecordDTO dto);
    }
}