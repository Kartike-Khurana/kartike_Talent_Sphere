using System;
using TalentSphere.Enums;

namespace TalentSphere.DTOs
{
    public class EmployeeResponseDto
    {
        public int EmployeeID { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public DateTime? JoinDate { get; set; }
        public EmployeeStatus Status { get; set; }
        public int? ManagerID { get; set; }
        public string? ManagerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
