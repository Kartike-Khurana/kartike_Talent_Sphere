using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentSphere.Models;
using TalentSphere.Enums;

namespace TalentSphere.Config.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(n => n.NotificationID);

            builder.Property(n => n.Message).IsRequired();
            builder.Property(n => n.Category).HasMaxLength(100);

            builder.Property(n => n.Status).HasDefaultValue(NotificationStatus.Unread).IsRequired();

            builder.Property(n => n.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserID).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
