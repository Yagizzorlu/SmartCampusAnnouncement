using SmartCampusAnnouncement.Application.Abstractions.Notifications;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Factories;

public sealed class NotificationChannelFactory : INotificationChannelFactory
{
    private readonly IReadOnlyDictionary<NotificationType, INotificationChannel> _channels;

    public NotificationChannelFactory(IEnumerable<INotificationChannel> channels)
    {
        ArgumentNullException.ThrowIfNull(channels);

        _channels = channels
            .GroupBy(channel => channel.NotificationType)
            .ToDictionary(
                group => group.Key,
                group => group.First());
    }

    public INotificationChannel Create(NotificationType notificationType)
    {
        if (!Enum.IsDefined(notificationType))
            throw new ArgumentOutOfRangeException(nameof(notificationType), "Invalid notification type.");

        if (_channels.TryGetValue(notificationType, out INotificationChannel? channel))
            return channel;

        throw new InvalidOperationException(
            $"No notification channel implementation was registered for notification type '{notificationType}'.");
    }
}
