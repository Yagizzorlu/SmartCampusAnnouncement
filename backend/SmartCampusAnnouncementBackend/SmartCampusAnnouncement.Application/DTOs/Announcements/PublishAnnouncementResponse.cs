namespace SmartCampusAnnouncement.Application.DTOs.Announcements;

public sealed record PublishAnnouncementResponse
{
    public int AnnouncementId { get; init; }

    public int TotalRecipients { get; init; }

    public int SentCount { get; init; }

    public int FailedCount { get; init; }

    public DateTime PublishedAt { get; init; }
}
