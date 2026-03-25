using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateJobDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Requirements { get; set; }
    }
}