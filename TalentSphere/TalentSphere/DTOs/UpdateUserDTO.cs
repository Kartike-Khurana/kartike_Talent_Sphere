using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // PasswordHash is optional on update; include only when resetting password
        //public string PasswordHash { get; set; }

        public string Phone { get; set; }

        // Make Status nullable so mapping can ignore it when not provided
        public UserStatus? Status { get; set; }
    }
}
