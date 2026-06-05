using SmartCampusAnnouncement.Application.Models.Observers;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Abstractions.Observers;

public interface IAnnouncementObserver
{
    UserType ObservedUserType { get; }

    Task<ObserverNotificationResult> NotifyAsync(Announcement announcement, CancellationToken cancellationToken = default);
}
