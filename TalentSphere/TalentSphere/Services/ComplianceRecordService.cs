using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Models;

namespace TalentSphere.Services
{
    public class ComplianceRecordService : IComplianceRecordService
    {
        private readonly IComplianceRecordRepository _complianceRecordRepository;
        private readonly IMapper _mapper;

        public ComplianceRecordService(IComplianceRecordRepository complianceRecordRepository, IMapper mapper)
        {
            _complianceRecordRepository = complianceRecordRepository;
            _mapper = mapper;
        }

        public async Task<CreateComplianceRecordDTO> CreateComplianceRecordAsync(CreateComplianceRecordDTO dto)
        {
            var record = _mapper.Map<ComplianceRecord>(dto);
            var result = await _complianceRecordRepository.AddComplianceRecordAsync(record);
            return _mapper.Map<CreateComplianceRecordDTO>(result);
        }
    }
}