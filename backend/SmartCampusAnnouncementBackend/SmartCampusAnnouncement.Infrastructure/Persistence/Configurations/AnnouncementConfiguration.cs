using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Infrastructure.Persistence.Configurations;

public sealed class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
{
    public void Configure(EntityTypeBuilder<Announcement> builder)
    {
        builder.ToTable("Announcements");

        builder.HasKey(announcement => announcement.Id);

        builder.Property(announcement => announcement.Title).IsRequired().HasMaxLength(200);

        builder.Property(announcement => announcement.Content).IsRequired().HasMaxLength(2000);

        builder.Property(announcement => announcement.AnnouncementType).IsRequired();

        builder.Property(announcement => announcement.TargetAudience).IsRequired();

        builder.Property(announcement => announcement.Status).IsRequired().HasDefaultValue(AnnouncementStatus.Draft);

        builder.Property(announcement => announcement.Priority).IsRequired().HasDefaultValue(AnnouncementPriority.Normal);

        builder.Property(announcement => announcement.PublishedAt).IsRequired(false);

        builder.Property(announcement => announcement.ExpiresAt).IsRequired(false);

        builder.Property(announcement => announcement.CreatedBy).IsRequired().HasMaxLength(100);

        builder.Property(announcement => announcement.CreatedAt).IsRequired();

        builder.HasMany(announcement => announcement.NotificationLogs).WithOne(log => log.Announcement).HasForeignKey(log => log.AnnouncementId).OnDelete(DeleteBehavior.Cascade);
    }
}
