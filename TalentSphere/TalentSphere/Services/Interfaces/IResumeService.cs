using System.Threading.Tasks;
using TalentSphere.Models;
using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IResumeService
    {
        Task<Resume> CreateResumeAsync(CreateResumeDTO dto);
        Task<Resume> GetByIdAsync(int id);
    }
}