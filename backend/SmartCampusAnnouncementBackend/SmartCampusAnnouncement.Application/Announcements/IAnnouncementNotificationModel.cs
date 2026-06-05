using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Announcements;

public interface IAnnouncementNotificationModel
{
    AnnouncementType AnnouncementType { get; }

    string BuildNotificationMessage(AppUser recipient);
}
