using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Domain.Entities;

public class NotificationLog : BaseEntity
{
    public int AppUserId { get; set; }

    public AppUser AppUser { get; set; } = null!;

    public int AnnouncementId { get; set; }

    public Announcement Announcement { get; set; } = null!;

    public NotificationType NotificationType { get; set; }

    public NotificationStatus Status { get; set; } = NotificationStatus.Sent;

    public string Message { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
