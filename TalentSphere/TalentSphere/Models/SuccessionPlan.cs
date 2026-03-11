using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
	public class SuccessionPlan
	{
		public int SuccessionId { get; set; }

		public int EmployeeId { get; set; }

		public string Position { get; set; }

		public string Timeline { get; set; }
		public SuccessionStatus status { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

	}
}
