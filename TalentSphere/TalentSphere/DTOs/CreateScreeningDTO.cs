using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class CreateScreeningDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "A valid ApplicationID is required.")]
        public int ApplicationID { get; set; }

        // Set server-side from JWT — not required from client
        public int RecruiterID { get; set; }

        [Required]
        public ScreeningResult Result { get; set; }

        [MaxLength(2000)]
        public string? Feedback { get; set; }
    }
}
