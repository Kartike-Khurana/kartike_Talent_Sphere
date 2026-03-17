using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
	public class SuccessionPlanService : ISuccessionPlanService
	{
		private readonly ISuccessionPlanRepository _repository;
		private readonly IMapper _mapper;

		public SuccessionPlanService(ISuccessionPlanRepository repository
			, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}
		public async Task<SuccessionPlan> CreateSuccessionPlanAsync(CreateSuccessionPlanDTO dto)
		{
			var succession = _mapper.Map<SuccessionPlan>(dto);
			succession.CreatedAt = DateTime.UtcNow;

			var added = await _repository.AddAsync(succession);
			await _repository.SaveChangesAsync();

			return added;
		}

		public async Task<SuccessionPlan> GetByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id);

		}
	}
}
