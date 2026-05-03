namespace TalentSphere.DTOs
{
    public class EnrollmentResponseDTO
    {
        public int EnrollmentID { get; set; }
        public int EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public int TrainingID { get; set; }
        public string? TrainingTitle { get; set; }
        public string? TrainingType { get; set; }
        public string? DeliveryMode { get; set; }
        public string? TrainingLink { get; set; }
        public string? ClassStartTime { get; set; }
        public string? ClassEndTime { get; set; }
        public DateTime? TrainingStartDate { get; set; }
        public DateTime? TrainingEndDate { get; set; }
        public DateOnly Date { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? Score { get; set; }
        public string? Notes { get; set; }
        public string? CertificateUrl { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsOverdue { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
