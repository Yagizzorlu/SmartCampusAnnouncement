using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Domain.Entities;

public class Announcement : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public AnnouncementType AnnouncementType { get; set; }

    public TargetAudience TargetAudience { get; set; }

    public AnnouncementStatus Status { get; set; } = AnnouncementStatus.Draft;

    public AnnouncementPriority Priority { get; set; } = AnnouncementPriority.Normal;

    public DateTime? PublishedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string CreatedBy { get; set; } = "System Admin";

    public ICollection<NotificationLog> NotificationLogs { get; set; } = new List<NotificationLog>();
}
