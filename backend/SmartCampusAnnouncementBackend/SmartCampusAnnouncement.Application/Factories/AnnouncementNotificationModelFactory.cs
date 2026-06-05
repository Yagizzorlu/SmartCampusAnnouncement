using SmartCampusAnnouncement.Application.Abstractions.Factories;
using SmartCampusAnnouncement.Application.Announcements;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Factories;

public sealed class AnnouncementNotificationModelFactory : IAnnouncementNotificationModelFactory
{
    public IAnnouncementNotificationModel Create(Announcement announcement)
    {
        ArgumentNullException.ThrowIfNull(announcement);

        return announcement.AnnouncementType switch
        {
            AnnouncementType.Exam => new ExamAnnouncement(announcement.Title, announcement.Content, announcement.Priority),
            AnnouncementType.Event => new EventAnnouncement(announcement.Title, announcement.Content, announcement.Priority),
            AnnouncementType.FoodMenu => new FoodMenuAnnouncement(announcement.Title, announcement.Content, announcement.Priority),
            AnnouncementType.Library => new LibraryAnnouncement(announcement.Title, announcement.Content, announcement.Priority),
            _ => throw new ArgumentOutOfRangeException(nameof(announcement.AnnouncementType), $"No notification model defined for announcement type '{announcement.AnnouncementType}'.")
        };
    }
}
