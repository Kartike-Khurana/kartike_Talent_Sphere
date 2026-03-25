using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Config;

namespace TalentSphere.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) => _context = context;

        public async Task<Role> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            return role;
        }

        public async Task<Role> GetByIdAsync(int id) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.RoleID == id && !r.IsDeleted);

        public async Task<Role> GetByNameAsync(string name) =>
            await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToString() == name && !r.IsDeleted);

        public async Task<IEnumerable<Role>> GetAllAsync() =>
            await _context.Roles.Where(r => !r.IsDeleted).ToListAsync();

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
        }

        public async Task DeleteAsync(Role role)
        {
            role.IsDeleted = true;
            _context.Roles.Update(role);
        }

        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
