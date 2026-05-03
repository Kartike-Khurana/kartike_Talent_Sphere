using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateJobDTO
    {
        public string? Title { get; set; }

        public string? Department { get; set; }

        public string? Description { get; set; }

        public string? Requirements { get; set; }

        public JobStatus? Status { get; set; }
    }
}
