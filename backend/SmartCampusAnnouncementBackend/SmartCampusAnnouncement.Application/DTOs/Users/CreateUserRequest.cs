using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.DTOs.Users;

public sealed record CreateUserRequest
{
    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string? PhoneNumber { get; init; }

    public UserType UserType { get; init; }

    public IReadOnlyCollection<NotificationType> NotificationTypes { get; init; } = Array.Empty<NotificationType>();
}
