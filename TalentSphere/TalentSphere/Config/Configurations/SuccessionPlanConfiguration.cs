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
            builder.HasKey(s => s.SuccessionID);
            builder.Property(s => s.SuccessionID).ValueGeneratedOnAdd();

            builder.Property(s => s.Status).HasColumnName("status").HasConversion<string>().HasDefaultValue(SuccessionStatus.Planned);

            builder.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(s => s.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property<bool>("IsDeleted").HasDefaultValue(false);

            builder.HasOne(s => s.Employee).WithMany().HasForeignKey(s => s.EmployeeID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Successor).WithMany().HasForeignKey(s => s.SuccessorID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
