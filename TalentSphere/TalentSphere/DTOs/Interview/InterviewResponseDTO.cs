using TalentSphere.Enums;

namespace TalentSphere.DTOs.Interview
{
    public class InterviewResponseDTO
    {
        public int InterviewID { get; set; }
        public int ApplicationID { get; set; }
        public int JobID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int CandidateID { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Location { get; set; }
        public int InterviewerID { get; set; }
        public string InterviewerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
