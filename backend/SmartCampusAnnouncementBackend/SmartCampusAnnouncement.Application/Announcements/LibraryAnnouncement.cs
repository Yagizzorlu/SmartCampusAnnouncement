using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Announcements;

public sealed class LibraryAnnouncement : AnnouncementNotificationModelBase
{
    public LibraryAnnouncement(string title, string content, AnnouncementPriority priority) : base(title, content, priority)
    {
    }

    public override AnnouncementType AnnouncementType => AnnouncementType.Library;

    public override string BuildNotificationMessage(AppUser recipient)
        => $"[KÜTÜPHANE] Sayın {recipient.FullName}, [{Priority}] {Title}: {Content}";
}
