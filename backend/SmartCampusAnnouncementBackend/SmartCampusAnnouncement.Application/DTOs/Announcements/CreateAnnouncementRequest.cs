using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.DTOs.Announcements;

public sealed record CreateAnnouncementRequest
{
    public string Title { get; init; } = string.Empty;

    public string Content { get; init; } = string.Empty;

    public AnnouncementType AnnouncementType { get; init; }

    public TargetAudience TargetAudience { get; init; }

    public AnnouncementPriority? Priority { get; init; }

    public DateTime? ExpiresAt { get; init; }

    public string? CreatedBy { get; init; }
}
