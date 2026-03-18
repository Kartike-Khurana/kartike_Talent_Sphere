using TalentSphere.DTOs;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces
{
	public interface IEnrollmentService
	{
		Task<Enrollment> CreateEnrollmentAsync(CreateEnrollmentDTO dto);
		Task<Enrollment> GetByIdAsync(int id);

	}
}
