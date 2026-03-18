using System;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class CreateApplicationDTO
    {
        public int JobID { get; set; }

        public int CandidateID { get; set; }

        public DateTime SubmittedDate { get; set; }

        public ApplicationStatus Status { get; set; }
    }
}