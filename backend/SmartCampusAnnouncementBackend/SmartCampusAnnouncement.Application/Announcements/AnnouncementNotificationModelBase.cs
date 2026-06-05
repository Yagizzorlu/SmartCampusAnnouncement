using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Announcements;

public abstract class AnnouncementNotificationModelBase : IAnnouncementNotificationModel
{
    protected AnnouncementNotificationModelBase(string title, string content, AnnouncementPriority priority)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        Title = title;
        Content = content;
        Priority = priority;
    }

    protected string Title { get; }

    protected string Content { get; }

    protected AnnouncementPriority Priority { get; }

    public abstract AnnouncementType AnnouncementType { get; }

    public abstract string BuildNotificationMessage(Domain.Entities.AppUser recipient);
}
