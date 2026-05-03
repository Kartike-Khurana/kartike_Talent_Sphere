using TalentSphere.Enums;

namespace TalentSphere.DTOs.Application
{
    public class ApplicationFilterParams
    {
        public int? JobID { get; set; }
        public int? CandidateID { get; set; }
        public ApplicationStatus? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
