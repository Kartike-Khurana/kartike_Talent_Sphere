using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs.Interview
{
    public class UpdateInterviewStatusDTO
    {
        [Required]
        public InterviewStatus Status { get; set; }

        [MaxLength(2000)]
        public string? Feedback { get; set; }
    }
}
