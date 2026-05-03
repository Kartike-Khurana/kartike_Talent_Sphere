namespace TalentSphere.DTOs
{
    public class ScreeningResponseDTO
    {
        public int ScreeningID { get; set; }
        public int ApplicationID { get; set; }
        public string? CandidateName { get; set; }
        public string? JobTitle { get; set; }
        public int RecruiterID { get; set; }
        public string Result { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
