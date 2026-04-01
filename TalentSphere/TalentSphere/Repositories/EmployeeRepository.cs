using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TalentSphere.Config;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Models;

namespace TalentSphere.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            var entity = (await _context.Set<Employee>().AddAsync(employee)).Entity;
            return entity;
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Set<Employee>()
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.EmployeeID == id && !EF.Property<bool>(e, "IsDeleted"));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Set<Employee>()
                .Include(e => e.User)
                .AsNoTracking()
                .Where(e => !EF.Property<bool>(e, "IsDeleted"))
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Employee>()
                .Include(e => e.User)
                .Where(e => e.UserId == userId && !EF.Property<bool>(e, "IsDeleted"))
                .ToListAsync();
        }
    }
}
