using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _repository;
        private readonly IMapper _mapper;

        public AuditService(IAuditRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AuditResponseDTO> CreateAuditAsync(CreateAuditDTO dto, int hrUserId)
        {
            var audit = _mapper.Map<Audit>(dto);
            audit.HRID = hrUserId;
            await _repository.AddAuditAsync(audit);
            return _mapper.Map<AuditResponseDTO>(audit);
        }

        public async Task<AuditResponseDTO> UpdateAuditAsync(int id, UpdateAuditDTO dto)
        {
            var audit = await _repository.GetAuditByIdAsync(id);
            if (audit == null) return null;
            _mapper.Map(dto, audit);
            audit.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAuditAsync(audit);
            return _mapper.Map<AuditResponseDTO>(audit);
        }

        public async Task<AuditResponseDTO> GetAuditByIdAsync(int id)
        {
            var audit = await _repository.GetAuditByIdAsync(id);
            return audit == null ? null : _mapper.Map<AuditResponseDTO>(audit);
        }

        public async Task<IEnumerable<AuditResponseDTO>> GetAllAuditsAsync()
        {
            var audits = await _repository.GetAllAuditsAsync();
            return audits.Select(a => _mapper.Map<AuditResponseDTO>(a));
        }

        public async Task<bool> DeleteAuditAsync(int id)
        {
            return await _repository.DeleteAuditAsync(id);
        }
    }
}
