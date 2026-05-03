using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class CreateScreeningDTO
    {
        public int ApplicationID { get; set; }
        public int RecruiterID { get; set; }
        public ScreeningResult Result { get; set; }
        public string? Feedback { get; set; }
    }
}
