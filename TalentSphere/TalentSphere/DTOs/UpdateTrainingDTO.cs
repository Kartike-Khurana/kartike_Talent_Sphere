using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class UpdateTrainingDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Duration { get; set; }
    }
}
