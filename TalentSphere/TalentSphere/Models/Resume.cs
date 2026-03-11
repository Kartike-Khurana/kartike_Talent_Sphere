using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
    public class Resume
        {
            public int ResumeID { get; set; }

            public int CandidateID { get; set; }

            public virtual User Candidate { get; set; } = null!;

            public string FileURI { get; set; } = string.Empty;

            public DateTime UploadedDate { get; set; } = DateTime.UtcNow;

            public ResumeStatus Status { get; set; } = ResumeStatus.Active;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
        }
}