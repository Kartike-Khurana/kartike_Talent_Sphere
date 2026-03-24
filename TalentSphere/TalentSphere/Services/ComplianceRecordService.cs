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

        public async Task<CreateComplianceRecordDTO> CreateComplianceRecordAsync(CreateComplianceRecordDTO createComplianceRecordDTO)
        {
            var record = _mapper.Map<ComplianceRecord>(createComplianceRecordDTO);
            var result = await _complianceRecordRepository.AddComplianceRecordAsync(record);
            return _mapper.Map<CreateComplianceRecordDTO>(result);
        }

        public async Task<UpdateComplianceRecordDTO> UpdateComplianceRecordAsync(int id, UpdateComplianceRecordDTO updateComplianceRecordDTO)
        {
            var record = await _complianceRecordRepository.GetComplianceRecordByIdAsync(id);
            if (record == null) return null;
            _mapper.Map(updateComplianceRecordDTO, record);
            await _complianceRecordRepository.UpdateComplianceRecordAsync(record);
            return _mapper.Map<UpdateComplianceRecordDTO>(record);
        }

        public async Task<ComplianceRecordResponseDTO> GetComplianceRecordByIdAsync(int id)
        {
            var record = await _complianceRecordRepository.GetComplianceRecordByIdAsync(id);
            return record == null ? null : _mapper.Map<ComplianceRecordResponseDTO>(record);
        }

        public async Task<IEnumerable<ComplianceRecordResponseDTO>> GetAllComplianceRecordsAsync()
        {
            var records = await _complianceRecordRepository.GetAllComplianceRecordsAsync();
            return records.Select(r => _mapper.Map<ComplianceRecordResponseDTO>(r));
        }

        public async Task<bool> DeleteComplianceRecordAsync(int id)
        {
            return await _complianceRecordRepository.DeleteComplianceRecordAsync(id);
        }
    }
}