namespace TalentSphere.DTOs
{
	public class CreateReportDTO
	{
		public string Scope { get; set; }
		public string Metrics { get; set; }
		public DateOnly GenerateDate { get; set; }
	}
}
