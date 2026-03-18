using System;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class CreateResumeDTO
    {
        public int CandidateID { get; set; }
        public string FileURI { get; set; }
        public DateTime UploadedDate { get; set; }
        public ResumeStatus Status { get; set; }
    }
}