using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class UpdateSuccesionPlanDTO
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Timeline { get; set; }

    }
}
