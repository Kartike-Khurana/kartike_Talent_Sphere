namespace TalentSphere.DTOs
{
    public class TrainingResponseDTO
    {
        public int TrainingID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string TrainingType { get; set; } = string.Empty;
        public string DeliveryMode { get; set; } = string.Empty;
        public string? TrainingLink { get; set; }
        public string? Location { get; set; }
        public string? InstructorName { get; set; }
        public string? ClassStartTime { get; set; }
        public string? ClassEndTime { get; set; }
        public int? MaxCapacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DurationDays { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
