using System;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace TalentSphere.Models
{
    public class Audit
    {
        public int ComplianceID { get; set; }

        public int EmployeeID { get; set; }

        public string Type { get; set; } // Policy / Document

        public string Result { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public Employee Employee { get; set; }
    }
}