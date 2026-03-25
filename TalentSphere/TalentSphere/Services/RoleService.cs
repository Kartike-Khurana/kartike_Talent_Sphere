using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RoleResponseDTO> CreateAsync(CreateRoleDTO dto)
        {
            // Check if role already exists
            var existingRole = await _repository.GetByNameAsync(dto.Name);
            if (existingRole != null)
            {
                throw new InvalidOperationException($"Role '{dto.Name}' already exists.");
            }

            var role = _mapper.Map<Role>(dto);
            await _repository.AddAsync(role);
            await _repository.SaveChangesAsync();
            return _mapper.Map<RoleResponseDTO>(role);
        }

        public async Task<RoleResponseDTO> GetByIdAsync(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            return role == null ? null : _mapper.Map<RoleResponseDTO>(role);
        }

        public async Task<IEnumerable<RoleResponseDTO>> GetAllAsync()
        {
            var roles = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleResponseDTO>>(roles);
        }

        public async Task<RoleResponseDTO> UpdateAsync(int id, UpdateRoleDTO dto)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null) return null;
            _mapper.Map(dto, role);
            await _repository.UpdateAsync(role);
            await _repository.SaveChangesAsync();
            return _mapper.Map<RoleResponseDTO>(role);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _repository.GetByIdAsync(id);
            if (role == null) return false;
            await _repository.DeleteAsync(role);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
