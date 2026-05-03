namespace TalentSphere.DTOs.CareerPlan
{
    public class CareerPlanResponseDTO
    {
        public int PlanID { get; set; }
        public int EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
