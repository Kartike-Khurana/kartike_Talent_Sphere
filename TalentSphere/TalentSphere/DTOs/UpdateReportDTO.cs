using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class UpdateReportDTO
    {
        [Required]
        public string Scope { get; set; }
        [Required]
        public string Metrics { get; set; }
        [Required]
        public DateOnly GenerateDate { get; set; }
    }
}
