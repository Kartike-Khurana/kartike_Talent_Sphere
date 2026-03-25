using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class CreateRoleDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
