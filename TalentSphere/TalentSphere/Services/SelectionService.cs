using System;
using System.Threading.Tasks;
using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Services
{
    public class SelectionService : ISelectionService
    {
        private readonly ISelectionRepository _repository;
        private readonly IMapper _mapper;

        public SelectionService(ISelectionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Selection> CreateSelectionAsync(CreateSelectionDTO dto)
        {
            var selection = _mapper.Map<Selection>(dto);
            selection.CreatedAt = DateTime.UtcNow;

            var added = await _repository.AddAsync(selection);
            await _repository.SaveChangesAsync();
            return added;
        }

        public async Task<Selection> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}
