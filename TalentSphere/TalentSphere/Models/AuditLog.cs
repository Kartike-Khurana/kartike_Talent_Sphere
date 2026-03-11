using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Models;

namespace TalentSphere.Models
{
    public class AuditLog
    {
        public int AuditID { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }

        public string Action { get; set; }

        public string Resource { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
