using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;

namespace TalentSphere.Config.Configurations
{
    public class EmployeeDocConfiguration : IEntityTypeConfiguration<EmployeeDoc>
    {
        public void Configure(EntityTypeBuilder<EmployeeDoc> builder)
        {
            builder.ToTable("EmployeeDocs");
            builder.HasKey(d => d.DocumentID);

            builder.Property(d => d.DocType).HasMaxLength(100);
            builder.Property(d => d.FileURI).HasMaxLength(500);

            builder.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(d => d.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(d => d.Employee).WithMany().HasForeignKey(d => d.EmployeeID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
