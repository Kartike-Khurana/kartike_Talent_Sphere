using TalentSphere.Enums;
using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateComplianceRecordDTO
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public CompilanceRecordType Type { get; set; }

        [Required]
        [StringLength(100)]
        public string Result { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

    }
}
