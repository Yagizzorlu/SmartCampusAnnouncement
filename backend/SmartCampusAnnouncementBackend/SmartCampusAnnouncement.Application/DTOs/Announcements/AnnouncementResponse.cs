using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.DTOs.Announcements;

public sealed record AnnouncementResponse
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public AnnouncementType AnnouncementType { get; init; }

    public TargetAudience TargetAudience { get; init; }

    public AnnouncementStatus Status { get; init; }

    public AnnouncementPriority Priority { get; init; }

    public DateTime? PublishedAt { get; init; }

    public DateTime? ExpiresAt { get; init; }

    public string CreatedBy { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}
