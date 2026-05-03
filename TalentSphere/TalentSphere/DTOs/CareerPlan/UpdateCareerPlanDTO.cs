using TalentSphere.Enums;

namespace TalentSphere.DTOs.CareerPlan
{
    public class UpdateCareerPlanDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public CareerPlanStatus? Status { get; set; }
    }
}
