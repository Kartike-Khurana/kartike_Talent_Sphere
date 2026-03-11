using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;
 
namespace TalentSphere.Models
{
    public class Interview
    {
        public int InterviewID { get; set; }

        public int ApplicationID { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }

        public int InterviewerID { get; set; }

        public InterviewStatus Status { get; set; }

        public Application Application { get; set; }

        public User Interviewer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
 