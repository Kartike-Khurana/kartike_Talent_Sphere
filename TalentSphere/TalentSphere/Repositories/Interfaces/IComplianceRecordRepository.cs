using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IComplianceRecordRepository
    {
        Task<ComplianceRecord> AddComplianceRecordAsync(ComplianceRecord compilanceRecord);
        Task<ComplianceRecord> GetComplianceRecordByIdAsync(int id);
        Task<IEnumerable<ComplianceRecord>> GetAllComplianceRecordsAsync();
        Task UpdateComplianceRecordAsync(ComplianceRecord complianceRecord);
        Task<bool> DeleteComplianceRecordAsync(int id);
    }
}