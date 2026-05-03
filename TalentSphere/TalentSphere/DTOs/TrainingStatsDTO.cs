namespace TalentSphere.DTOs
{
    public class TrainingStatsDTO
    {
        public int TotalTrainings { get; set; }
        public int ActiveTrainings { get; set; }
        public int TotalEnrollments { get; set; }
        public int CompletedEnrollments { get; set; }
        public int OverdueEnrollments { get; set; }
        public double CompletionRate { get; set; }
    }
}
