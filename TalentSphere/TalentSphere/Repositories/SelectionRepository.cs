using System.Threading.Tasks;
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
            var entity = (await _context.Set<Selection>().AddAsync(selection)).Entity;
            return entity;
        }

        public async Task<Selection> GetByIdAsync(int id)
        {
            return await _context.Set<Selection>().FirstOrDefaultAsync(s => s.SelectionID == id && !EF.Property<bool>(s, "IsDeleted"));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
