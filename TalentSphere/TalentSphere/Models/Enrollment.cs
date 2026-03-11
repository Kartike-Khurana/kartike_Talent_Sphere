using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
	public class Enrollment
	{
		public int EnrollmentID { get; set; }

		public int TrainingID { get; set; }

		public int EmployeeID { get; set; }

		public DateOnly Date { get; set; }

		public EnrollmentStatus status { get; set; } = EnrollmentStatus.Enrolled;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

	}
}
