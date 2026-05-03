namespace TalentSphere.DTOs.Analytics
{
    public class TrainingAnalyticsDTO
    {
        public int TotalTrainings { get; set; }
        public int TotalEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
        public double OverallCompletionRate { get; set; }
        public IEnumerable<TrainingCompletionDTO> ByTraining { get; set; } = [];
    }

    public class TrainingCompletionDTO
    {
        public int TrainingID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int EnrollmentCount { get; set; }
        public int CompletedCount { get; set; }
        public double CompletionRate { get; set; }
    }
}
