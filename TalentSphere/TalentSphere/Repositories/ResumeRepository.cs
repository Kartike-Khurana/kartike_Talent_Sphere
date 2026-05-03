using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Repositories
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly AppDbContext _context;

        public ResumeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Resume> AddAsync(Resume resume)
        {
            return (await _context.Set<Resume>().AddAsync(resume)).Entity;
        }

        public async Task<Resume?> GetByIdAsync(int id)
        {
            return await _context.Set<Resume>()
                .Include(r => r.Candidate)
                .FirstOrDefaultAsync(r => r.ResumeID == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Resume>> GetAllAsync()
        {
            return await _context.Set<Resume>()
                .Include(r => r.Candidate)
                .AsNoTracking()
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.UploadedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Resume>> GetByCandidateIdAsync(int candidateId)
        {
            return await _context.Set<Resume>()
                .AsNoTracking()
                .Where(r => r.CandidateID == candidateId && !r.IsDeleted)
                .OrderByDescending(r => r.UploadedDate)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
