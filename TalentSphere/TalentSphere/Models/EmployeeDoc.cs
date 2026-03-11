using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Models;

namespace TalentSphere.Models
{
    public class EmployeeDoc
    {
        public int DocumentID { get; set; }

        public int EmployeeID { get; set; }

        public string DocType { get; set; }

        public string FileURI { get; set; }

        public DateTime? UploadedDate { get; set; }

        public string VerifStatus { get; set; } = "Pending";

        public Employee Employee { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
