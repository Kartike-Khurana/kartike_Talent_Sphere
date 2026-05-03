using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateAuditDTO
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime AuditDate { get; set; }

        public string Status { get; set; } = "Active";
    }
}
