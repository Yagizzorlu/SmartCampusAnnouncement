using SmartCampusAnnouncement.Application.Models.Notifications;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Abstractions.Notifications;

public interface INotificationChannel
{
    NotificationType NotificationType { get; }

    Task<NotificationDeliveryResult> SendAsync(AppUser recipient, Announcement announcement, CancellationToken cancellationToken = default);
}
