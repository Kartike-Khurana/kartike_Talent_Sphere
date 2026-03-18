using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
	public class EnrollmentService : IEnrollmentService
	{
		private readonly IEnrollmentRepository _repository;
		private readonly IMapper _mapper;
		public EnrollmentService(IEnrollmentRepository repository , IMapper mapper)
		{
			_repository = repository;
				_mapper = mapper;
		}
		public async Task<Enrollment> CreateEnrollmentAsync(CreateEnrollmentDTO dto)
		{
			var Enrollment = _mapper.Map<Enrollment>(dto);
			Enrollment.CreatedAt = DateTime.UtcNow;

			var added = await _repository.AddAsync(Enrollment);
			await _repository.SaveChangesAsync();
			return added;

		}
		public async Task<Enrollment> GetByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id);
		}
	}
}
