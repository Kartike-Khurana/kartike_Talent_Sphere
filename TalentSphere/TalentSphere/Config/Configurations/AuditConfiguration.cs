using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;

namespace TalentSphere.Config.Configurations
{
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audits");
            builder.HasKey(a => a.ComplianceID);

            builder.Property(a => a.Type).IsRequired().HasMaxLength(255);
            builder.Property(a => a.Result).IsRequired().HasMaxLength(255);
            builder.Property(a => a.Date).IsRequired();
            builder.Property(a => a.Notes).HasMaxLength(1000);

            builder.Property(a => a.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(a => a.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(a => a.Employee).WithMany().HasForeignKey(a => a.EmployeeID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
