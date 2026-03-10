using Microsoft.OpenApi;

namespace TalentSphere.Models
{
	public class Report
	{
		public int ReportID { get; set; }
		public string Scope { get; set; }
		public string Metrics { get; set; }
		public DateTime GeneratedDate { get; set; }
	}
}
