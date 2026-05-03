namespace TalentSphere.DTOs
{
    public class SuccessionPlanResponseDTO
    {
        public int SuccessionID { get; set; }
        public int EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public int SuccessorID { get; set; }
        public string? SuccessorName { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
