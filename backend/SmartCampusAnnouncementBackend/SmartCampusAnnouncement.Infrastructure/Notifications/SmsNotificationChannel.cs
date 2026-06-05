using SmartCampusAnnouncement.Application.Abstractions.Factories;
using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Notifications;
using SmartCampusAnnouncement.Application.Announcements;
using SmartCampusAnnouncement.Application.Models.Notifications;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Infrastructure.Notifications;

public sealed class SmsNotificationChannel : INotificationChannel
{
    private readonly IAnnouncementNotificationModelFactory _modelFactory;
    private readonly IAppLogger _logger;

    public SmsNotificationChannel(IAnnouncementNotificationModelFactory modelFactory, IAppLogger logger)
    {
        _modelFactory = modelFactory;
        _logger = logger;
    }

    public NotificationType NotificationType => NotificationType.Sms;

    public Task<NotificationDeliveryResult> SendAsync(AppUser recipient, Announcement announcement, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(recipient.PhoneNumber))
        {
            IAnnouncementNotificationModel model = _modelFactory.Create(announcement);
            string failedMessage = model.BuildNotificationMessage(recipient);
            _logger.Warning($"SMS notification failed because recipient '{recipient.FullName}' has no phone number.");
            return Task.FromResult(NotificationDeliveryResult.Failed(failedMessage, "Recipient phone number is empty."));
        }

        IAnnouncementNotificationModel notificationModel = _modelFactory.Create(announcement);
        string message = notificationModel.BuildNotificationMessage(recipient);
        _logger.Information($"[SMS] To: {recipient.PhoneNumber} | {message}");
        return Task.FromResult(NotificationDeliveryResult.Success(message));
    }
}
