using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
    public class Application
    {
        public int ApplicationID { get; set; }

        public int JobID { get; set; }

        public virtual Job Job { get; set; } = null!;

        public int CandidateID { get; set; }

        public virtual User Candidate { get; set; } = null!;

        public DateTime SubmittedDate { get; set; } = DateTime.UtcNow;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    
}
