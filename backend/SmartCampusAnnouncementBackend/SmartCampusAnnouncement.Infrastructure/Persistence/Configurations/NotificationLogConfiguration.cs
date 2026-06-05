using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Infrastructure.Persistence.Configurations;

public sealed class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
{
    public void Configure(EntityTypeBuilder<NotificationLog> builder)
    {
        builder.ToTable("NotificationLogs");

        builder.HasKey(log => log.Id);

        builder.Property(log => log.AppUserId).IsRequired();

        builder.Property(log => log.AnnouncementId).IsRequired();

        builder.Property(log => log.NotificationType).IsRequired();

        builder.Property(log => log.Status).IsRequired().HasDefaultValue(NotificationStatus.Sent);

        builder.Property(log => log.Message).IsRequired().HasMaxLength(1000);

        builder.Property(log => log.ErrorMessage).HasMaxLength(1000);

        builder.Property(log => log.SentAt).IsRequired();

        builder.Property(log => log.CreatedAt).IsRequired();
    }
}
