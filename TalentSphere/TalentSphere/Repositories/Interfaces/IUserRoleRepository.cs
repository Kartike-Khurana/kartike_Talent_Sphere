using System.Threading.Tasks;
using System.Collections.Generic;
using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddAsync(UserRole userRole);
        Task<UserRole> GetByIdAsync(int id);
        Task<UserRole> GetByUserIdAsync(int userId);
        Task<UserRole?> GetAnyByUserIdAsync(int userId);
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task UpdateAsync(UserRole userRole);
        Task SaveChangesAsync();

        /// <summary>Returns distinct active Users who hold any of the specified role names.</summary>
        Task<IEnumerable<User>> GetUsersByRolesAsync(IEnumerable<string> roleNames);
    }
}
