using System.Threading.Tasks;
using TalentSphere.DTOs;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces;

public interface IJobService
{
    Task<Job> CreateJobAsync(CreateJobDTO dto);
    Task<Job> GetByIdAsync(int id);
}
