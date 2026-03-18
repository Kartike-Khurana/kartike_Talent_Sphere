using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
	public class TrainingService : ITrainingService
	{
		private readonly ITrainingRepository _repository;
		private readonly IMapper _mapper;

		public TrainingService(ITrainingRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<Training> CreateTrainingAsync(CreateTrainingDTO dto)
		{
			var training = _mapper.Map<Training>(dto);
			training.CreatedAt = DateTime.UtcNow;

			var added = await _repository.AddAsync(training);
			await _repository.SaveChangesAsync();

			return added;
		}
		public async Task<Training> GetbyIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id);

		}
	}
}
