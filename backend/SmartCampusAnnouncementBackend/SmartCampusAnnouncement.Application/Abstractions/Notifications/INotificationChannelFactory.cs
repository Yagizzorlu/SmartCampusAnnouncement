using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Abstractions.Notifications;

public interface INotificationChannelFactory
{
    INotificationChannel Create(NotificationType notificationType);
}
