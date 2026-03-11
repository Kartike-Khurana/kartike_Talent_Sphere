using System;
using System.Collections.Generic;

namespace TalentSphere.Models
{
    public class Role
    {
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
