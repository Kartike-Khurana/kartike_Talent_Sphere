using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateUserRoleDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
