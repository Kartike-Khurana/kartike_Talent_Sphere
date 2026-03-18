using System.Threading.Tasks;
using TalentSphere.Models;
using TalentSphere.DTOs;

namespace TalentSphere.Services.Interfaces
{
    public interface IScreeningService
    {
        Task<Screening> CreateScreeningAsync(CreateScreeningDTO dto);
        Task<Screening> GetByIdAsync(int id);
    }
}