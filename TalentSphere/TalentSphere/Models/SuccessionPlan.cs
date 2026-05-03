using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
	public class SuccessionPlan
	{
		public int SuccessionID { get; set; }
		public int EmployeeID { get; set; }
		public virtual Employee Employee { get; set; }
		public int SuccessorID { get; set; }
		public virtual Employee? Successor { get; set; }
		public SuccessionStatus Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public bool IsDeleted { get; set; }
	}
}
