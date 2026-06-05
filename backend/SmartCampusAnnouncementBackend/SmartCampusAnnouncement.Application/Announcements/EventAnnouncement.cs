using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Announcements;

public sealed class EventAnnouncement : AnnouncementNotificationModelBase
{
    public EventAnnouncement(string title, string content, AnnouncementPriority priority) : base(title, content, priority)
    {
    }

    public override AnnouncementType AnnouncementType => AnnouncementType.Event;

    public override string BuildNotificationMessage(AppUser recipient)
        => $"[ETKİNLİK] Sayın {recipient.FullName}, [{Priority}] {Title}: {Content}";
}
