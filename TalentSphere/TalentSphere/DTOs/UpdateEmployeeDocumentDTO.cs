using System;
using System.ComponentModel.DataAnnotations;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class UpdateEmployeeDocumentDTO
    {
        // Make fields optional to support partial updates via PUT
        public int? EmployeeID { get; set; }

        public string? FileURI { get; set; }

        public DateTime? UploadedDate { get; set; }

        public EmployeeDocType? DocType { get; set; }

        public EmployeeDocVerifyStatus? VerifyStatus { get; set; }
    }
}
