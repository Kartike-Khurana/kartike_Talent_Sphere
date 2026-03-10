namespace TalentSphere.Models
{
	public class Enrollment
	{
		public int EnrollmentID { get; set; }
		public int TrainingID { get; set; }
		public Training Training { get; set; }
	
		public int EmployeeID { get; set; }
//public Employee Employee { get; set; }

		public DateOnly Date { get; set; }
		public string Status { get; set; }
	}
}
