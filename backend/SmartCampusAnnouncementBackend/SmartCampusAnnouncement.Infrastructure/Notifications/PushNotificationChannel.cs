using SmartCampusAnnouncement.Application.Abstractions.Factories;
using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Notifications;
using SmartCampusAnnouncement.Application.Announcements;
using SmartCampusAnnouncement.Application.Models.Notifications;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Infrastructure.Notifications;

public sealed class PushNotificationChannel : INotificationChannel
{
    private readonly IAnnouncementNotificationModelFactory _modelFactory;
    private readonly IAppLogger _logger;

    public PushNotificationChannel(IAnnouncementNotificationModelFactory modelFactory, IAppLogger logger)
    {
        _modelFactory = modelFactory;
        _logger = logger;
    }

    public NotificationType NotificationType => NotificationType.Push;

    public Task<NotificationDeliveryResult> SendAsync(AppUser recipient, Announcement announcement, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IAnnouncementNotificationModel notificationModel = _modelFactory.Create(announcement);
        string message = notificationModel.BuildNotificationMessage(recipient);
        _logger.Information($"[PUSH] User: {recipient.FullName} | {message}");
        return Task.FromResult(NotificationDeliveryResult.Success(message));
    }
}
