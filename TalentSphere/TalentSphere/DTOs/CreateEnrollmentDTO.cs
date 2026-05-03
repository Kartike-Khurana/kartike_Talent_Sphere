using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateEnrollmentDTO
    {
        [Required] public int EmployeeID { get; set; }
        [Required] public int TrainingID { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
