namespace TalentSphere.DTOs.PerformanceReview
{
    public class PerformanceReviewListDTO
    {
        public int ReviewID { get; set; }
        public int EmployeeID { get; set; }
        public string? EmployeeName { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? Comments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
