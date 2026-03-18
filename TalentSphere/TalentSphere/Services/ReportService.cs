using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
	public class ReportService : IReportService
	{
		private readonly IReportRepository _repository;
		private readonly IMapper _mapper;

		public ReportService(IReportRepository repository , IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}
		public async Task<Report> CreateReportAsync(CreateReportDTO dto)
		{
			var report = _mapper.Map<Report>(dto);
			report.CreatedAt = DateTime.UtcNow;

			var added = await _repository.AddAsync(report);
			await _repository.SaveChangesAsync();

			return added;
		}

		public async Task<Report> GetByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id);
		}
	}
}
