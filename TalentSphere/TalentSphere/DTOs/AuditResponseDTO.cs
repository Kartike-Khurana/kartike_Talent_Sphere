using System;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class AuditResponseDTO
    {
        public int AuditID { get; set; }
        public int HRID { get; set; }
        public string Scope { get; set; }
        public string Findings { get; set; }
        public DateTime Date { get; set; }
        public AuditStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        // Optionally, add HR details if needed
    }
}
