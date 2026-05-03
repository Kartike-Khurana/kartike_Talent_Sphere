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
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.EmployeeID == id && !e.IsDeleted);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Set<Employee>()
                .Include(e => e.User)
                .Include(e => e.Manager)
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Employee>()
                .Include(e => e.User)
                .Include(e => e.Manager)
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .ToListAsync();
        }
    }
}
