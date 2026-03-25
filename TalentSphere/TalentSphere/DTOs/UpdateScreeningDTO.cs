using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateScreeningDTO
    {
        [Required]
        public int ApplicationID { get; set; }
        
        [Required]
        public int RecruiterID { get; set; }
        
        [Required]
        public ScreeningResult? Result { get; set; }
        
        public string? Notes { get; set; }
        public DateTime? Date { get; set; }
    }
}
