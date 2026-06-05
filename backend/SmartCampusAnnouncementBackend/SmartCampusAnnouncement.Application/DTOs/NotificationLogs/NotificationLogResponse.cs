using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.DTOs.NotificationLogs;

public sealed record NotificationLogResponse
{
    public int Id { get; init; }

    public int AppUserId { get; init; }

    public string RecipientFullName { get; init; } = string.Empty;

    public UserType RecipientUserType { get; init; }

    public int AnnouncementId { get; init; }

    public string AnnouncementTitle { get; init; } = string.Empty;

    public AnnouncementType AnnouncementType { get; init; }

    public NotificationType NotificationType { get; init; }

    public NotificationStatus Status { get; init; }

    public string Message { get; init; } = string.Empty;

    public string? ErrorMessage { get; init; }

    public DateTime SentAt { get; init; }
}
