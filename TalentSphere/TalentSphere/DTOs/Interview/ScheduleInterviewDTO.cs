using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs.Interview
{
    public class ScheduleInterviewDTO
    {
        [Required]
        public int ApplicationID { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly Time { get; set; }

        [Required]
        public int InterviewerID { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }
    }
}
