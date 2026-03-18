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

        public async Task<ComplianceRecord> AddComplianceRecordAsync(ComplianceRecord record)
        {
            try
            {
                await _context.ComplianceRecords.AddAsync(record);
                await _context.SaveChangesAsync();
                return record;
            }
            catch (DbUpdateException ex)
            {
                
                throw new Exception("Database update failed. Please check if the EmployeeID exists and all fields are valid.", ex);
            }
            catch (Exception ex)
            {
            
                throw new Exception("An error occurred while saving the compliance record.", ex);
            }
        }
    }
}