using System;
using System.ComponentModel.DataAnnotations;

namespace TalentSphere.Models
{
	public class Report
	{

		public int ReportID { get; set; }

		public string Scope { get; set; }

		public string Metrics { get; set; }

		public DateOnly GenerateDate { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }
		
		public bool IsDeleted { get; set; }

	}
}
