using System.Collections.Generic;
using System.Threading.Tasks;
using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponseDTO> CreateAsync(CreateRoleDTO dto);
        Task<RoleResponseDTO> GetByIdAsync(int id);
        Task<IEnumerable<RoleResponseDTO>> GetAllAsync();
        Task<RoleResponseDTO> UpdateAsync(int id, UpdateRoleDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
