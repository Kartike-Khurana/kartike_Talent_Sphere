using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
    public class Screening
    {
        public int ScreeningID { get; set; }

        public int ApplicationID { get; set; }

        public virtual Application Application { get; set; } = null!;

        public int RecruiterID { get; set; }

        public virtual User Recruiter { get; set; } = null!;

        public ScreeningResult Result { get; set; } = ScreeningResult.Fail;

        public string? Notes { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}