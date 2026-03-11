using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;

namespace TalentSphere.Config.Configurations
{
    public class PerformanceReviewConfiguration : IEntityTypeConfiguration<PerformanceReview>
    {
        public void Configure(EntityTypeBuilder<PerformanceReview> builder)
        {
            builder.ToTable("PerformanceReviews");
            builder.HasKey(p => p.ReviewID);

            builder.Property(p => p.Score).HasColumnType("decimal(5,2)");
            builder.Property(p => p.Date).HasColumnType("datetime");

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(p => p.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.Employee).WithMany().HasForeignKey(p => p.EmployeeID).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.Manager).WithMany().HasForeignKey(p => p.ManagerID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
