
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentSphere.Models
{
    public class CareerPlan
    {
        public int PlanID { get; set; }

        public int EmployeeID { get; set; }

        public string Goals { get; set; }

        public string Timeline { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual Employee Employee { get; set; }
    }
}