using System;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Application> CreateApplicationAsync(CreateApplicationDTO dto)
        {
            var application = _mapper.Map<Application>(dto);

            application.CreatedAt = DateTime.UtcNow;
            application.IsDeleted = false;

            var added = await _repository.AddAsync(application);
            await _repository.SaveChangesAsync();
            return added;
        }

        public async Task<Application> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
