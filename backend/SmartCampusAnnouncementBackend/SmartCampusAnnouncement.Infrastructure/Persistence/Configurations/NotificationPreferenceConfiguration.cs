using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Infrastructure.Persistence.Configurations;

public sealed class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
    public void Configure(EntityTypeBuilder<NotificationPreference> builder)
    {
        builder.ToTable("NotificationPreferences");

        builder.HasKey(preference => preference.Id);

        builder.Property(preference => preference.AppUserId).IsRequired();

        builder.Property(preference => preference.NotificationType).IsRequired();

        builder.Property(preference => preference.CreatedAt).IsRequired();

        builder.HasIndex(preference => new { preference.AppUserId, preference.NotificationType }).IsUnique();
    }
}
