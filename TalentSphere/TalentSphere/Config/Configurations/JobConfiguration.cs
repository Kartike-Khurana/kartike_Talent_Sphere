using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;
using TalentSphere.Enums;

namespace TalentSphere.Config.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("Jobs");
            builder.HasKey(j => j.JobID);

            builder.Property(j => j.Title).IsRequired().HasMaxLength(255);
            builder.Property(j => j.Department).IsRequired().HasMaxLength(100);
            builder.Property(j => j.Description).HasMaxLength(2000);
            builder.Property(j => j.Requirements).HasMaxLength(2000);

            builder.Property(j => j.Status)
                    .HasConversion<string>()
                   .HasDefaultValue(JobStatus.Open);
        }
    }
}
