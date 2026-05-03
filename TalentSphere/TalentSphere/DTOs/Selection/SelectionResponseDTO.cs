namespace TalentSphere.DTOs.Selection
{
    public class SelectionResponseDTO
    {
        public int SelectionID { get; set; }
        public int ApplicationID { get; set; }
        public string Decision { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime Date { get; set; }
        public int CandidateID { get; set; }
        public string CandidateName { get; set; } = string.Empty;
        public string CandidateEmail { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public int? CreatedEmployeeID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
