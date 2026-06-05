using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Notifications;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Application.Abstractions.Time;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Observers;

public sealed class TeacherAnnouncementObserver : AnnouncementObserverBase
{
    public TeacherAnnouncementObserver(IUserRepository userRepository, INotificationLogRepository notificationLogRepository, INotificationChannelFactory notificationChannelFactory, IDateTimeProvider dateTimeProvider, IAppLogger logger)
        : base(userRepository, notificationLogRepository, notificationChannelFactory, dateTimeProvider, logger)
    {
    }

    public override UserType ObservedUserType => UserType.Teacher;
}
