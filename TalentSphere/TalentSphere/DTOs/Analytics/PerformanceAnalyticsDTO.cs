namespace TalentSphere.DTOs.Analytics
{
    public class PerformanceAnalyticsDTO
    {
        public int TotalReviews { get; set; }
        public double OverallAverageScore { get; set; }
        public IEnumerable<DepartmentPerformanceDTO> ByDepartment { get; set; } = [];
        public IEnumerable<TopPerformerDTO> TopPerformers { get; set; } = [];
    }

    public class DepartmentPerformanceDTO
    {
        public string Department { get; set; } = string.Empty;
        public double AverageScore { get; set; }
        public int ReviewCount { get; set; }
    }

    public class TopPerformerDTO
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public double AverageScore { get; set; }
        public int ReviewCount { get; set; }
    }
}
