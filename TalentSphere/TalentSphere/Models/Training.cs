using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
	public class Training
	{
		public int TrainingID { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Duration { get; set; }

		public TrainingStatus status { get; set; } = TrainingStatus.Planned;
        
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
		public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


	}
}