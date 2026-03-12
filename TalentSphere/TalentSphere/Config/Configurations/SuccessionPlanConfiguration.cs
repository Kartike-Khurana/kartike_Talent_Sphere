using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;
using TalentSphere.Enums;

namespace TalentSphere.Config.Configurations
{
    public class SuccessionPlanConfiguration : IEntityTypeConfiguration<SuccessionPlan>
    {
        public void Configure(EntityTypeBuilder<SuccessionPlan> builder)
        {
            builder.ToTable("SuccessionPlans");
            builder.HasKey(s => s.SuccessionId);

            builder.Property(s => s.Position).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Timeline).IsRequired().HasMaxLength(255);

            builder.Property(s => s.status).HasConversion<string>().HasDefaultValue(SuccessionStatus.Planned);

            builder.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(s => s.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne<Employee>().WithMany().HasForeignKey(s => s.EmployeeId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
