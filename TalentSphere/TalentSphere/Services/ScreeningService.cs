using System;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Services
{
    public class ScreeningService : IScreeningService
    {
        private readonly IScreeningRepository _repository;
        private readonly IMapper _mapper;

        public ScreeningService(IScreeningRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Screening> CreateScreeningAsync(CreateScreeningDTO dto)
        {
            var screening = _mapper.Map<Screening>(dto);
            screening.CreatedAt = DateTime.UtcNow;
            screening.IsDeleted = false;

            var added = await _repository.AddAsync(screening);
            await _repository.SaveChangesAsync();
            return added;
        }

        public async Task<Screening> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}