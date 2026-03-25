using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateEmployeeDocumentDTO
    {
        [Required]
        public int? EmployeeID { get; set; }
        [Required]
        public string FileURI { get; set; }
        [Required]
        public DateTime? UploadedDate { get; set; }
        [Required]
        public EmployeeDocType? DocType { get; set; }
        public EmployeeDocVerifyStatus? VerifyStatus { get; set; }
    }
}
