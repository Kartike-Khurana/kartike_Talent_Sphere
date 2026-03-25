using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateApplicationDTO
    {
        [Required]
        public int JobID { get; set; }

        [Required]
        public int CandidateID { get; set; }

        // Allow SubmittedDate to be null when not updating
        public DateTime? SubmittedDate { get; set; }

        // Make Status nullable so mapping can ignore it when not provided
        public ApplicationStatus? Status { get; set; }
    }
}
