using System.Collections.Generic;
using System.Threading.Tasks;
using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> AddAsync(Role role);
        Task<Role> GetByIdAsync(int id);
        Task<Role> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task SaveChangesAsync();
    }
}
