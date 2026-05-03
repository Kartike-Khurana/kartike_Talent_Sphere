using TalentSphere.Enums;

namespace TalentSphere.DTOs.Job
{
    public class JobFilterParams
    {
        public string? Search { get; set; }
        public string? Department { get; set; }
        public JobStatus? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
