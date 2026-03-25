using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class UpdateEnrollmentDTO
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public int TrainingID { get; set; }
    }
}
