using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Announcements;

public sealed class FoodMenuAnnouncement : AnnouncementNotificationModelBase
{
    public FoodMenuAnnouncement(string title, string content, AnnouncementPriority priority) : base(title, content, priority)
    {
    }

    public override AnnouncementType AnnouncementType => AnnouncementType.FoodMenu;

    public override string BuildNotificationMessage(AppUser recipient)
        => $"[YEMEKHANE] Merhaba {recipient.FullName}, [{Priority}] {Title}: {Content}";
}
