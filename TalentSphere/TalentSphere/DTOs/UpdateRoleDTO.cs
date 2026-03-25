using System.ComponentModel.DataAnnotations;

namespace TalentSphere.DTOs
{
    public class UpdateRoleDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
