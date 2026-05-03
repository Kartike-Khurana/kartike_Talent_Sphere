namespace TalentSphere.DTOs
{
    public class UpdateTrainingDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? TrainingType { get; set; }
        public string? DeliveryMode { get; set; }
        public string? TrainingLink { get; set; }
        public string? Location { get; set; }
        public string? InstructorName { get; set; }
        public string? ClassStartTime { get; set; }
        public string? ClassEndTime { get; set; }
        public int? MaxCapacity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }
    }
}
