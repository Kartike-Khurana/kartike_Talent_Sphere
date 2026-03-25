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
                await _context.Audits.AddAsync(audit);
                await _context.SaveChangesAsync();
                return audit;
            
        }

        public async Task<Audit> GetAuditByIdAsync(int id)
        {
            return await _context.Audits.FirstOrDefaultAsync(a => a.AuditID == id && !a.IsDeleted);
        }

        public async Task<IEnumerable<Audit>> GetAllAuditsAsync()
        {

            return await _context.Audits.Where(a => !a.IsDeleted).ToListAsync();
        }

        public async Task UpdateAuditAsync(Audit audit)
        {
            _context.Audits.Update(audit);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAuditAsync(int id)
        {
            var audit = await _context.Audits.FindAsync(id);
            if (audit == null || audit.IsDeleted) return false;
            audit.IsDeleted = true;
            _context.Audits.Update(audit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}