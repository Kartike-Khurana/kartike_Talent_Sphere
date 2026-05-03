using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs.Selection
{
    public class MakeSelectionDecisionDTO
    {
        [Required]
        public int ApplicationID { get; set; }

        [Required]
        public SelectionDecision Decision { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public string? Department { get; set; }

        public string? Position { get; set; }
    }
}
