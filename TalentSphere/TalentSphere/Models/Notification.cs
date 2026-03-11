
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentSphere.Enums;

namespace TalentSphere.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }

        public int UserID { get; set; }

        public int EntityID { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public NotificationStatus Status { get; set; } = NotificationStatus.Unread;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
    }
}