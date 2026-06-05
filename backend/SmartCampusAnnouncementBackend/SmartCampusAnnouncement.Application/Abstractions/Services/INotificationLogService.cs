using SmartCampusAnnouncement.Application.DTOs.NotificationLogs;

namespace SmartCampusAnnouncement.Application.Abstractions.Services;

public interface INotificationLogService
{
    Task<IReadOnlyList<NotificationLogResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<NotificationLogSummaryResponse> GetSummaryAsync(CancellationToken cancellationToken = default);
}
