using System.Threading.Tasks;
using TalentSphere.Models;
using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<Application> CreateApplicationAsync(CreateApplicationDTO dto);
        Task<Application> GetByIdAsync(int id);
    }
}