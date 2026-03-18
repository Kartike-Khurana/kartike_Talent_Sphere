using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IComplianceRecordRepository
    {
        Task<ComplianceRecord> AddComplianceRecordAsync(ComplianceRecord record);
    }
}