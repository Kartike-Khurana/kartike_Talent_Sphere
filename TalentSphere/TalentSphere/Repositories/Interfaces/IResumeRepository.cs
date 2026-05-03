using TalentSphere.Models;

namespace TalentSphere.Repositories.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume> AddAsync(Resume resume);
        Task<Resume?> GetByIdAsync(int id);
        Task<IEnumerable<Resume>> GetAllAsync();
        Task<IEnumerable<Resume>> GetByCandidateIdAsync(int candidateId);
        Task SaveChangesAsync();
    }
}