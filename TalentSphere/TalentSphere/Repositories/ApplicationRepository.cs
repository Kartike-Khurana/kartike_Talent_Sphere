using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.DTOs.Application;
using TalentSphere.DTOs.Common;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application> AddAsync(Application application)
        {
            return (await _context.Set<Application>().AddAsync(application)).Entity;
        }

        public async Task<Application?> GetByIdAsync(int id)
        {
            return await _context.Set<Application>()
                .FirstOrDefaultAsync(a => a.ApplicationID == id && !a.IsDeleted);
        }

        public async Task<Application?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Application>()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .FirstOrDefaultAsync(a => a.ApplicationID == id && !a.IsDeleted);
        }

        public async Task<IEnumerable<Application>> GetAllAsync()
        {
            return await _context.Set<Application>()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .AsNoTracking()
                .Where(a => !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<PagedResult<Application>> GetPagedAsync(ApplicationFilterParams filters)
        {
            var query = _context.Set<Application>()
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => !a.IsDeleted)
                .AsQueryable();

            if (filters.JobID.HasValue)
                query = query.Where(a => a.JobID == filters.JobID.Value);

            if (filters.CandidateID.HasValue)
                query = query.Where(a => a.CandidateID == filters.CandidateID.Value);

            if (filters.Status.HasValue)
                query = query.Where(a => a.Status == filters.Status.Value);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderByDescending(a => a.SubmittedDate)
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResult<Application>
            {
                Data = data,
                Page = filters.Page,
                PageSize = filters.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<IEnumerable<Application>> GetByJobIdAsync(int jobId)
        {
            return await _context.Set<Application>()
                .Include(a => a.Candidate)
                .Where(a => a.JobID == jobId && !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetByCandidateIdAsync(int candidateId)
        {
            return await _context.Set<Application>()
                .Include(a => a.Job)
                .Where(a => a.CandidateID == candidateId && !a.IsDeleted)
                .OrderByDescending(a => a.SubmittedDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
