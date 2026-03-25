using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class CreateEmployeeDocumentDTO
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public EmployeeDocType DocType { get; set; }
        [Required]
        public DateTime? UploadedDate { get; set; }
    }
}
