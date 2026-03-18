using System;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository _repository;
        private readonly IMapper _mapper;

        public InterviewService(IInterviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Interview> CreateInterviewAsync(CreateInterviewDTO dto)
        {
            var interview = _mapper.Map<Interview>(dto);
            interview.CreatedAt = DateTime.UtcNow;

            var added = await _repository.AddAsync(interview);
            await _repository.SaveChangesAsync();
            return added;
        }

        public async Task<Interview> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
