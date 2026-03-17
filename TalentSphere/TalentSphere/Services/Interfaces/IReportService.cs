using TalentSphere.DTOs;
using TalentSphere.Models;

namespace TalentSphere.Services.Interfaces
{
	public interface IReportService
	{
		Task<Report> CreateReportAsync(CreateReportDTO dto);
		Task<Report> GetByIdAsync(int id);
	}
}
