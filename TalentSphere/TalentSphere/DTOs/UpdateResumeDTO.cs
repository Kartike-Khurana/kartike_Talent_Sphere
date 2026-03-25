using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateResumeDTO
    {
        [Required]
        public int CandidateID { get; set; }

        [Required]
        public string FileURI { get; set; }

        // Allow UploadedDate to be null when not updating
        public DateTime? UploadedDate { get; set; }

        // Make Status nullable so mapping can ignore it when not provided
        public ResumeStatus? Status { get; set; }
    }
}
