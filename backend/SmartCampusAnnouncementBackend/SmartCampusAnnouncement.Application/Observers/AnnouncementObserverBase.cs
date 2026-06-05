using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Notifications;
using SmartCampusAnnouncement.Application.Abstractions.Observers;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Application.Abstractions.Time;
using SmartCampusAnnouncement.Application.Models.Observers;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Observers;

public abstract class AnnouncementObserverBase : IAnnouncementObserver
{
    private readonly IUserRepository _userRepository;
    private readonly INotificationLogRepository _notificationLogRepository;
    private readonly INotificationChannelFactory _notificationChannelFactory;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAppLogger _logger;

    protected AnnouncementObserverBase(IUserRepository userRepository, INotificationLogRepository notificationLogRepository, INotificationChannelFactory notificationChannelFactory, IDateTimeProvider dateTimeProvider, IAppLogger logger)
    {
        _userRepository = userRepository;
        _notificationLogRepository = notificationLogRepository;
        _notificationChannelFactory = notificationChannelFactory;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public abstract UserType ObservedUserType { get; }

    public async Task<ObserverNotificationResult> NotifyAsync(Announcement announcement, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(announcement);

        if (!IsAnnouncementTargetingObservedUserType(announcement))
        {
            _logger.Information($"[Observer] {GetType().Name}: TargetAudience={announcement.TargetAudience} → skipped for {ObservedUserType}.");
            return ObserverNotificationResult.Empty;
        }

        IReadOnlyList<AppUser> users = await _userRepository.GetActiveUsersByTypeWithPreferencesAsync(ObservedUserType, cancellationToken);

        if (users.Count == 0)
        {
            _logger.Warning($"[Observer] {GetType().Name}: TargetAudience={announcement.TargetAudience}, targeted {ObservedUserType}=0 → no active users found.");
            return ObserverNotificationResult.Empty;
        }

        int sentCount = 0;
        int failedCount = 0;
        List<NotificationLog> notificationLogs = new();

        foreach (AppUser user in users)
        {
            IReadOnlyCollection<NotificationPreference> preferences = user.NotificationPreferences
                .GroupBy(preference => preference.NotificationType)
                .Select(group => group.First())
                .ToList();

            if (preferences.Count == 0)
            {
                _logger.Warning($"[Observer] {GetType().Name}: user '{user.FullName}' has no notification preference → skipped.");
                continue;
            }

            foreach (NotificationPreference preference in preferences)
            {
                NotificationLog notificationLog = await SendNotificationAndCreateLogAsync(user, announcement, preference.NotificationType, cancellationToken);
                notificationLogs.Add(notificationLog);

                if (notificationLog.Status == NotificationStatus.Sent)
                    sentCount++;
                else
                    failedCount++;
            }
        }

        if (notificationLogs.Count > 0)
            await _notificationLogRepository.AddRangeAsync(notificationLogs, cancellationToken);

        _logger.Information($"[Observer] {GetType().Name}: TargetAudience={announcement.TargetAudience}, targeted {ObservedUserType}={users.Count}, sent={sentCount}, failed={failedCount}.");

        return ObserverNotificationResult.Create(targetedUserCount: users.Count, sentCount: sentCount, failedCount: failedCount);
    }

    private async Task<NotificationLog> SendNotificationAndCreateLogAsync(AppUser user, Announcement announcement, NotificationType notificationType, CancellationToken cancellationToken)
    {
        try
        {
            INotificationChannel channel = _notificationChannelFactory.Create(notificationType);
            var deliveryResult = await channel.SendAsync(user, announcement, cancellationToken);

            return new NotificationLog
            {
                AppUserId = user.Id,
                AnnouncementId = announcement.Id,
                NotificationType = notificationType,
                Status = deliveryResult.IsSuccess ? NotificationStatus.Sent : NotificationStatus.Failed,
                Message = deliveryResult.Message,
                ErrorMessage = deliveryResult.ErrorMessage,
                SentAt = _dateTimeProvider.UtcNow
            };
        }
        catch (Exception exception)
        {
            string fallbackMessage = BuildFallbackNotificationMessage(user, announcement, notificationType);

            _logger.Error($"Notification failed for user '{user.FullName}' via '{notificationType}'.", exception);

            return new NotificationLog
            {
                AppUserId = user.Id,
                AnnouncementId = announcement.Id,
                NotificationType = notificationType,
                Status = NotificationStatus.Failed,
                Message = fallbackMessage,
                ErrorMessage = exception.Message,
                SentAt = _dateTimeProvider.UtcNow
            };
        }
    }

    private bool IsAnnouncementTargetingObservedUserType(Announcement announcement)
    {
        return announcement.TargetAudience switch
        {
            TargetAudience.All => true,
            TargetAudience.Students => ObservedUserType == UserType.Student,
            TargetAudience.Teachers => ObservedUserType == UserType.Teacher,
            _ => false
        };
    }

    private static string BuildFallbackNotificationMessage(AppUser user, Announcement announcement, NotificationType notificationType)
    {
        return $"Notification could not be delivered to '{user.FullName}' via '{notificationType}' for announcement '{announcement.Title}'.";
    }
}
