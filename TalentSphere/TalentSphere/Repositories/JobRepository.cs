using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.DTOs.Common;
using TalentSphere.DTOs.Job;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly AppDbContext _context;

        public JobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Job> AddAsync(Job job)
        {
            return (await _context.Jobs.AddAsync(job)).Entity;
        }

        public async Task<Job?> GetByIdAsync(int id)
        {
            return await _context.Jobs
                .FirstOrDefaultAsync(j => j.JobID == id && !j.IsDeleted);
        }

        public async Task<List<Job>> GetAllAsync()
        {
            return await _context.Jobs
                .Where(j => !j.IsDeleted)
                .OrderByDescending(j => j.PostedDate)
                .ToListAsync();
        }

        public async Task<PagedResult<Job>> GetPagedAsync(JobFilterParams filters)
        {
            var query = _context.Jobs.Where(j => !j.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Search))
            {
                var search = filters.Search.ToLower();
                query = query.Where(j =>
                    j.Title.ToLower().Contains(search) ||
                    j.Department.ToLower().Contains(search) ||
                    j.Description.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(filters.Department))
                query = query.Where(j => j.Department.ToLower() == filters.Department.ToLower());

            if (filters.Status.HasValue)
                query = query.Where(j => j.Status == filters.Status.Value);

            var totalCount = await query.CountAsync();
            var data = await query
                .OrderByDescending(j => j.PostedDate)
                .Skip((filters.Page - 1) * filters.PageSize)
                .Take(filters.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResult<Job>
            {
                Data = data,
                Page = filters.Page,
                PageSize = filters.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task UpdateAsync(Job job)
        {
            _context.Jobs.Update(job);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Job job)
        {
            job.IsDeleted = true;
            job.UpdatedAt = DateTime.UtcNow;
            _context.Jobs.Update(job);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
