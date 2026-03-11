using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;

namespace TalentSphere.Config.Configurations
{
    public class CareerPlanConfiguration : IEntityTypeConfiguration<CareerPlan>
    {
        public void Configure(EntityTypeBuilder<CareerPlan> builder)
        {
            builder.ToTable("CareerPlans");
            builder.HasKey(c => c.PlanID);

            builder.Property(c => c.Goals).IsRequired();
            builder.Property(c => c.Timeline).HasMaxLength(255);

            builder.Property(c => c.Status).HasMaxLength(50);

            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(c => c.Employee).WithMany().HasForeignKey(c => c.EmployeeID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
