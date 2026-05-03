using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs.CareerPlan
{
    public class CreateCareerPlanDTO
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        public CareerPlanStatus Status { get; set; } = CareerPlanStatus.Planned;
    }
}
