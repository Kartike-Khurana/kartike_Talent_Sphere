using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateComplianceRecordDTO
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public CompilanceRecordType Type { get; set; }

        [Required]
        [StringLength(100)]
        public string Result { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}
