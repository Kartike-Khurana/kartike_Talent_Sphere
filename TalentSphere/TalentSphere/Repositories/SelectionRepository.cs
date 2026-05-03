using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Repositories
{
    public class SelectionRepository : ISelectionRepository
    {
        private readonly AppDbContext _context;

        public SelectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Selection> AddAsync(Selection selection)
        {
            return (await _context.Set<Selection>().AddAsync(selection)).Entity;
        }

        public async Task<Selection?> GetByIdAsync(int id)
        {
            return await _context.Set<Selection>()
                .FirstOrDefaultAsync(s => s.SelectionID == id && !s.IsDeleted);
        }

        public async Task<Selection?> GetByApplicationIdAsync(int applicationId)
        {
            return await _context.Set<Selection>()
                .Include(s => s.Application)
                    .ThenInclude(a => a.Job)
                .Include(s => s.Application)
                    .ThenInclude(a => a.Candidate)
                .FirstOrDefaultAsync(s => s.ApplicationID == applicationId && !s.IsDeleted);
        }

        public async Task<List<Selection>> GetAllAsync()
        {
            return await _context.Set<Selection>()
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Selection>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Selection>()
                .Include(s => s.Application)
                    .ThenInclude(a => a.Job)
                .Include(s => s.Application)
                    .ThenInclude(a => a.Candidate)
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Selection selection)
        {
            _context.Set<Selection>().Update(selection);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Selection selection)
        {
            selection.IsDeleted = true;
            selection.UpdatedAt = DateTime.UtcNow;
            _context.Set<Selection>().Update(selection);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
