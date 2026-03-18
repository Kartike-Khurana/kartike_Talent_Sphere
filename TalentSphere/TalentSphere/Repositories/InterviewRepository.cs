using System.Threading.Tasks;
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
            var entity = (await _context.Set<Interview>().AddAsync(interview)).Entity;
            return entity;
        }

        public async Task<Interview> GetByIdAsync(int id)
        {
            return await _context.Set<Interview>().FirstOrDefaultAsync(i => i.InterviewID == id && !EF.Property<bool>(i, "IsDeleted"));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
