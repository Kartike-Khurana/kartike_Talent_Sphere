using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateUserDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Phone { get; set; }

    }
}
