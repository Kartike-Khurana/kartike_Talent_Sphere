using AutoMapper;
using TalentSphere.DTOs;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Repositories.Interfaces;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IResumeRepository _repository;
        private readonly IFileStorageService _fileStorage;
        private readonly IMapper _mapper;

        public ResumeService(
            IResumeRepository repository,
            IFileStorageService fileStorage,
            IMapper mapper)
        {
            _repository = repository;
            _fileStorage = fileStorage;
            _mapper = mapper;
        }

        public async Task<ResumeResponseDTO> UploadResumeAsync(int candidateId, IFormFile file)
        {
            var uri = await _fileStorage.SaveResumeAsync(file, candidateId);

            var resume = new Resume
            {
                CandidateID  = candidateId,
                FileURI      = uri,
                UploadedDate = DateTime.UtcNow,
                Status       = ResumeStatus.Active,
                CreatedAt    = DateTime.UtcNow,
                IsDeleted    = false
            };

            await _repository.AddAsync(resume);
            await _repository.SaveChangesAsync();
            return _mapper.Map<ResumeResponseDTO>(resume);
        }

        public async Task<ResumeResponseDTO?> GetByIdAsync(int id)
        {
            var resume = await _repository.GetByIdAsync(id);
            return resume is null ? null : _mapper.Map<ResumeResponseDTO>(resume);
        }

        public async Task<IEnumerable<ResumeResponseDTO>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ResumeResponseDTO>>(list);
        }

        public async Task<IEnumerable<ResumeResponseDTO>> GetByCandidateIdAsync(int candidateId)
        {
            var list = await _repository.GetByCandidateIdAsync(candidateId);
            return _mapper.Map<IEnumerable<ResumeResponseDTO>>(list);
        }

        public async Task<ResumeResponseDTO?> ReplaceFileAsync(int id, IFormFile file)
        {
            var resume = await _repository.GetByIdAsync(id);
            if (resume is null) return null;

            // Delete old file from disk first
            _fileStorage.DeleteFile(resume.FileURI);

            // Save new file
            var uri = await _fileStorage.SaveResumeAsync(file, resume.CandidateID);
            resume.FileURI      = uri;
            resume.UploadedDate = DateTime.UtcNow;
            resume.UpdatedAt    = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
            return _mapper.Map<ResumeResponseDTO>(resume);
        }

        public async Task<bool> DeleteResumeAsync(int id)
        {
            var resume = await _repository.GetByIdAsync(id);
            if (resume is null) return false;

            // Soft-delete the record; leave the file on disk
            // (file cleanup can be a background job; avoids accidental data loss)
            resume.IsDeleted  = true;
            resume.UpdatedAt  = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
