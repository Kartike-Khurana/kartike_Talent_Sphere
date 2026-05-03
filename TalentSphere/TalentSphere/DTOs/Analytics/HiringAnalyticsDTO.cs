namespace TalentSphere.DTOs.Analytics
{
    public class HiringAnalyticsDTO
    {
        public int TotalJobs { get; set; }
        public int TotalApplications { get; set; }
        public int TotalHired { get; set; }
        public double AverageApplicationsPerJob { get; set; }
        public IEnumerable<JobApplicationCountDTO> ApplicationsPerJob { get; set; } = [];
    }

    public class JobApplicationCountDTO
    {
        public int JobID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int ApplicationCount { get; set; }
        public int HiredCount { get; set; }
    }
}
