using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs.PerformanceReview
{
    public class UpdatePerformanceReviewDTO
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }

        [MaxLength(1000)]
        public string? Comments { get; set; }

        public DateTime? ReviewDate { get; set; }
    }
}
