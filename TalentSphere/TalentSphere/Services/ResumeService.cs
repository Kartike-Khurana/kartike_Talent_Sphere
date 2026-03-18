using System;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _repository;
        private readonly IMapper _mapper;

        public ResumeService(IResumeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Resume> CreateResumeAsync(CreateResumeDTO dto)
        {
            var resume = _mapper.Map<Resume>(dto);
            resume.CreatedAt = DateTime.UtcNow;
            resume.IsDeleted = false;

            var added = await _repository.AddAsync(resume);
            await _repository.SaveChangesAsync();
            return added;
        }

        public async Task<Resume> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}