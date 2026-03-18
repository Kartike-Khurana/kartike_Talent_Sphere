using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class CreateAuditDTO
    {
        [Required]
        public int HRID { get; set; }

        [Required]
        [StringLength(500)]
        public string Scope { get; set; }

        public string Findings { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public AuditStatus Status { get; set; }
    }
}