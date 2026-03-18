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

    public async Task<CreateAuditDTO> CreateAuditAsync(CreateAuditDTO dto)
    {
        var audit = _mapper.Map<Audit>(dto);

        await _repository.AddAuditAsync(audit);

        return _mapper.Map<CreateAuditDTO>(audit);
    }
}
}