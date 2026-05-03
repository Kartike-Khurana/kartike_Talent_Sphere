using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs.PerformanceReview
{
    public class CreatePerformanceReviewDTO
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comments { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }
    }
}
