using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Application.Abstractions.Persistence;

public interface INotificationLogRepository
{
    Task<IReadOnlyList<NotificationLog>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(NotificationLog notificationLog, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<NotificationLog> notificationLogs, CancellationToken cancellationToken = default);
}
