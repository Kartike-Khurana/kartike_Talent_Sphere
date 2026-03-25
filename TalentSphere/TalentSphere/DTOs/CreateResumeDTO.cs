using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class CreateResumeDTO
    {
        [Required]
        public int CandidateID { get; set; }
        [Required]
        public string FileURI { get; set; }
        [Required]
        public DateTime UploadedDate { get; set; }
        [Required]
        public ResumeStatus Status { get; set; }
    }
}