using TalentSphere.Repositories.Interfaces;
using TalentSphere.Config;
using TalentSphere.Models;
using Microsoft.EntityFrameworkCore;

namespace TalentSphere.Repositories
{
    public class ComplianceRecordRepository : IComplianceRecordRepository
    {
        private readonly AppDbContext _context;

        public ComplianceRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ComplianceRecord> AddComplianceRecordAsync(ComplianceRecord compilanceRecord)
        {
                await _context.ComplianceRecords.AddAsync(compilanceRecord);
                await _context.SaveChangesAsync();
                return compilanceRecord;
        }

        public async Task<ComplianceRecord> GetComplianceRecordByIdAsync(int id)
        {
            return await _context.ComplianceRecords.FirstOrDefaultAsync(c => c.ComplianceID == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<ComplianceRecord>> GetAllComplianceRecordsAsync()
        {
            return await _context.ComplianceRecords.Where(c => !c.IsDeleted).ToListAsync();
        }

        public async Task UpdateComplianceRecordAsync(ComplianceRecord complianceRecord)
        {
            complianceRecord.UpdatedAt = DateTime.UtcNow;
            _context.ComplianceRecords.Update(complianceRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteComplianceRecordAsync(int ComplianceID)
        {
            var record = await _context.ComplianceRecords.FindAsync(ComplianceID);
            if (record == null || record.IsDeleted) return false;
            record.IsDeleted = true;
            _context.ComplianceRecords.Update(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}