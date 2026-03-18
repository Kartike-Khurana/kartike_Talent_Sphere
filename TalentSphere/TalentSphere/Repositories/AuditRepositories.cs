using TalentSphere.Repositories.Interfaces;
using TalentSphere.Config;
using TalentSphere.Models;
using Microsoft.EntityFrameworkCore;

namespace TalentSphere.Repositories
{
    public class AuditRepository : IAuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Audit> AddAuditAsync(Audit audit)
        {
            try
            {
                await _context.Audits.AddAsync(audit);
                await _context.SaveChangesAsync();
                return audit;
            }
            catch (DbUpdateException ex)
            {

                throw new Exception("Database update failed. Please check if the HRID exists and all required fields are provided.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the audit record.", ex);
            }
        }
    }
}