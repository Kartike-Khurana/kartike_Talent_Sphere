using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace TalentSphere.DTOs
{
	public class CreateSuccessionPlanDTO
	{
		public int EmployeeID { get; set; }
		public string Position { get; set; }
		public string Timeline { get; set; }


	}
}
