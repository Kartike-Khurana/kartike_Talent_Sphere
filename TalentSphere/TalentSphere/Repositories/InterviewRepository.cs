using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Repositories
{
    public class InterviewRepository : IInterviewRepository
    {
        private readonly AppDbContext _context;

        public InterviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Interview> AddAsync(Interview interview)
        {
            return (await _context.Set<Interview>().AddAsync(interview)).Entity;
        }

        public async Task<Interview?> GetByIdAsync(int id)
        {
            return await _context.Set<Interview>()
                .FirstOrDefaultAsync(i => i.InterviewID == id && !i.IsDeleted);
        }

        public async Task<Interview?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Interview>()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .FirstOrDefaultAsync(i => i.InterviewID == id && !i.IsDeleted);
        }

        public async Task<List<Interview>> GetAllAsync()
        {
            return await _context.Set<Interview>()
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Interview>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Interview>()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i => !i.IsDeleted)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Interview>> GetByApplicationIdAsync(int applicationId)
        {
            return await _context.Set<Interview>()
                .Include(i => i.Application)
                    .ThenInclude(a => a.Job)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Candidate)
                .Include(i => i.Interviewer)
                .Where(i => i.ApplicationID == applicationId && !i.IsDeleted)
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Interview interview)
        {
            _context.Set<Interview>().Update(interview);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Interview interview)
        {
            interview.IsDeleted = true;
            interview.UpdatedAt = DateTime.UtcNow;
            _context.Set<Interview>().Update(interview);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
