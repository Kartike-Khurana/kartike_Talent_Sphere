namespace TalentSphere.DTOs
{
    public class CreateSuccessionPlanDTO
    {
        public int EmployeeID { get; set; }
        public int SuccessorID { get; set; }
        public string Status { get; set; } = "Planned";
    }
}
