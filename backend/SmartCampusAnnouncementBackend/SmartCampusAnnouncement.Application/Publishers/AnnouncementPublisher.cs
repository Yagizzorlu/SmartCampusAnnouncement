using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Observers;
using SmartCampusAnnouncement.Application.Abstractions.Publishing;
using SmartCampusAnnouncement.Application.DTOs.Announcements;
using SmartCampusAnnouncement.Application.Models.Observers;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Application.Publishers;

public sealed class AnnouncementPublisher : IAnnouncementPublisher
{
    private readonly IReadOnlyCollection<IAnnouncementObserver> _observers;
    private readonly IAppLogger _logger;

    public AnnouncementPublisher(IEnumerable<IAnnouncementObserver> observers, IAppLogger logger)
    {
        ArgumentNullException.ThrowIfNull(observers);

        _observers = observers.ToList();
        _logger = logger;
    }

    public async Task<PublishAnnouncementResponse> PublishAsync(Announcement announcement, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(announcement);

        if (_observers.Count == 0)
            _logger.Warning($"Announcement '{announcement.Title}' is being published, but no observers are registered.");

        int totalRecipients = 0;
        int sentCount = 0;
        int failedCount = 0;

        foreach (IAnnouncementObserver observer in _observers)
        {
            ObserverNotificationResult result = await observer.NotifyAsync(announcement, cancellationToken);

            totalRecipients += result.TargetedUserCount;
            sentCount += result.SentCount;
            failedCount += result.FailedCount;
        }

        _logger.Information($"Announcement '{announcement.Title}' published. Recipients: {totalRecipients}, Sent: {sentCount}, Failed: {failedCount}");

        return new PublishAnnouncementResponse
        {
            AnnouncementId = announcement.Id,
            TotalRecipients = totalRecipients,
            SentCount = sentCount,
            FailedCount = failedCount,
            PublishedAt = announcement.PublishedAt ?? DateTime.UtcNow
        };
    }
}
