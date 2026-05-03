using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateTrainingDTO
    {
        [Required] public string Title { get; set; }
        public string? Description { get; set; }
        [Required] public string TrainingType { get; set; }
        [Required] public string DeliveryMode { get; set; }
        public string? TrainingLink { get; set; }
        public string? Location { get; set; }
        public string? InstructorName { get; set; }
        public string? ClassStartTime { get; set; }
        public string? ClassEndTime { get; set; }
        public int? MaxCapacity { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Planned";
    }
}
