using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateEmployeeDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }

        public DateTime? JoinDate { get; set; }

        public EmployeeStatus? Status { get; set; }
    }
}
