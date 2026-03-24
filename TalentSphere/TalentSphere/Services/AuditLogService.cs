using System;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repository;
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // CreateAuditLogAsync removed: audit logs should not be created via public service API.

        public async Task<AuditLogResponseDto> GetByIdAsync(int id)
        {
            var audit = await _repository.GetByIdAsync(id);
            if (audit == null) return null;
            return _mapper.Map<AuditLogResponseDto>(audit);
        }

        public async Task<IEnumerable<AuditLogResponseDto>> GetAllAsync()
        {
            var audits = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuditLogResponseDto>>(audits);
        }
        // Update operation removed: audit logs are immutable via the public service API.

        public async Task<bool> DeleteAuditLogAsync(int id)
        {
            var audit = await _repository.GetByIdAsync(id);
            if (audit == null) return false;

            audit.IsDeleted = true;
            audit.UpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
