using System;
using TalentSphere.Enums;
using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateApplicationDTO
    {
        [Required]
        public int JobID { get; set; }

        [Required]
        public int CandidateID { get; set; }

        [Required]
        public DateTime SubmittedDate { get; set; }

        [Required]
        public ApplicationStatus Status { get; set; }
    }
}