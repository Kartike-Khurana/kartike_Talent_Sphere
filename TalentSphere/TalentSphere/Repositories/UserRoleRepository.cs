using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.Repositories.Interfaces; 
using TalentSphere.Models;

namespace TalentSphere.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;

        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserRole> AddAsync(UserRole userRole)
        {
            var entity = (await _context.Set<UserRole>().AddAsync(userRole)).Entity;
            return entity;
        }

        public async Task<UserRole> GetByIdAsync(int id)
        {
            return await _context.Set<UserRole>().FirstOrDefaultAsync(ur => ur.UserRoleId == id && !EF.Property<bool>(ur, "IsDeleted"));
        }

        public async Task<UserRole> GetByUserIdAsync(int userId)
        {
            return await _context.Set<UserRole>()
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId && !EF.Property<bool>(ur, "IsDeleted"))
                .OrderByDescending(ur => ur.UserRoleId)
                .FirstOrDefaultAsync();
        }

        public async Task<UserRole?> GetAnyByUserIdAsync(int userId)
        {
            return await _context.Set<UserRole>()
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId)
                .OrderByDescending(ur => ur.UserRoleId)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            return await _context.Set<UserRole>()
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .AsNoTracking()
                .Where(ur => !EF.Property<bool>(ur, "IsDeleted"))
                .ToListAsync();
        }

        public async Task UpdateAsync(UserRole userRole)
        {
            _context.Set<UserRole>().Update(userRole);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<User>> GetUsersByRolesAsync(IEnumerable<string> roleNames)
        {
            var roleNameSet = new HashSet<string>(roleNames, StringComparer.OrdinalIgnoreCase);

            // Resolve role IDs in memory — avoids EF Core failing to translate
            // enum-with-string-converter comparisons to SQL.
            var allRoles = await _context.Set<Role>().AsNoTracking().ToListAsync();
            var roleIds = allRoles
                .Where(r => !r.IsDeleted && roleNameSet.Contains(r.Name.ToString()))
                .Select(r => r.RoleID)
                .ToList();

            if (!roleIds.Any()) return [];

            // Integer IN-list — EF Core translates this trivially.
            var userRoles = await _context.Set<UserRole>()
                .Include(ur => ur.User)
                .Where(ur => !ur.IsDeleted && roleIds.Contains(ur.RoleId))
                .AsNoTracking()
                .ToListAsync();

            return userRoles
                .Where(ur => !ur.User.IsDeleted && ur.User.Status == Enums.UserStatus.Active)
                .Select(ur => ur.User)
                .DistinctBy(u => u.UserID)
                .ToList();
        }
    }
}
