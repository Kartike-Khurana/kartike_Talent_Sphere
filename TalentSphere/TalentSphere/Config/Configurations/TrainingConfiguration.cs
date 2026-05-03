using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Enums;
using TalentSphere.Models;

namespace TalentSphere.Config.Configurations
{
    public class TrainingConfiguration : IEntityTypeConfiguration<Training>
    {
        public void Configure(EntityTypeBuilder<Training> builder)
        {
            builder.ToTable("Trainings");
            builder.HasKey(t => t.TrainingID);
            builder.Property(t => t.TrainingID).ValueGeneratedOnAdd();

            builder.Property(t => t.Title).IsRequired().HasMaxLength(255);
            builder.Property(t => t.Description).HasMaxLength(2000);
            builder.Property(t => t.TrainingType).HasConversion<string>().IsRequired();
            builder.Property(t => t.DeliveryMode).HasConversion<string>().IsRequired();
            builder.Property(t => t.TrainingLink).HasMaxLength(500);
            builder.Property(t => t.Location).HasMaxLength(255);
            builder.Property(t => t.InstructorName).HasMaxLength(255);
            builder.Property(t => t.ClassStartTime).HasMaxLength(10);
            builder.Property(t => t.ClassEndTime).HasMaxLength(10);
            builder.Property(t => t.status).HasConversion<string>().HasDefaultValue(TrainingStatus.Planned).IsRequired();

            builder.Property(t => t.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(t => t.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property<bool>("IsDeleted").HasDefaultValue(false);
        }
    }
}
