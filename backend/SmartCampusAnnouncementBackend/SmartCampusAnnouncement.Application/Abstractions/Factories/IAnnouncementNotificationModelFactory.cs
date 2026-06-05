using SmartCampusAnnouncement.Application.Announcements;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Application.Abstractions.Factories;

public interface IAnnouncementNotificationModelFactory
{
    IAnnouncementNotificationModel Create(Announcement announcement);
}
