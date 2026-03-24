using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Models;

namespace TalentSphere.Services{
    public class AuditService : IAuditService
{
    private readonly IAuditRepository _repository;
    private readonly IMapper _mapper;

    public AuditService(IAuditRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CreateAuditDTO> CreateAuditAsync(CreateAuditDTO createAuditDTO)
    {
        var audit = _mapper.Map<Audit>(createAuditDTO);

        await _repository.AddAuditAsync(audit);

        return _mapper.Map<CreateAuditDTO>(audit);
    }

        public async Task<UpdateAuditDTO> UpdateAuditAsync(int id, UpdateAuditDTO updateAuditDTO)
    {
        var audit = await _repository.GetAuditByIdAsync(id);
        if (audit == null) return null;
        _mapper.Map(updateAuditDTO, audit);
        await _repository.UpdateAuditAsync(audit);
        return _mapper.Map<UpdateAuditDTO>(audit);
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