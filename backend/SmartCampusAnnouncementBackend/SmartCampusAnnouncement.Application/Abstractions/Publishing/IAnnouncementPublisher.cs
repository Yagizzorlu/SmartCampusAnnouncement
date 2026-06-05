using SmartCampusAnnouncement.Application.DTOs.Announcements;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Application.Abstractions.Publishing;

public interface IAnnouncementPublisher
{
    Task<PublishAnnouncementResponse> PublishAsync(Announcement announcement, CancellationToken cancellationToken = default);
}
