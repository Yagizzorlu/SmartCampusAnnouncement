using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Announcements;

public sealed class ExamAnnouncement : AnnouncementNotificationModelBase
{
    public ExamAnnouncement(string title, string content, AnnouncementPriority priority) : base(title, content, priority)
    {
    }

    public override AnnouncementType AnnouncementType => AnnouncementType.Exam;

    public override string BuildNotificationMessage(AppUser recipient)
        => $"[SINAV DUYURUSU] Sayın {recipient.FullName}, [{Priority}] {Title}: {Content}";
}
