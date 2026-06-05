using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Domain.Entities;

public class NotificationPreference : BaseEntity
{
    public int AppUserId { get; set; }

    public AppUser AppUser { get; set; } = null!;

    public NotificationType NotificationType { get; set; }
}
