using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;
using TalentSphere.Enums;

namespace TalentSphere.Config.Configurations
{
    public class SelectionConfiguration : IEntityTypeConfiguration<Selection>
    {
        public void Configure(EntityTypeBuilder<Selection> builder)
        {
            builder.ToTable("Selections");
            builder.HasKey(s => s.SelectionID);

            builder.Property(s => s.Decision).IsRequired();

            builder.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(s => s.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(s => s.Application).WithMany().HasForeignKey(s => s.ApplicationID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
