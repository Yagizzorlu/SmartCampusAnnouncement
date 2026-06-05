using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.DTOs.NotificationLogs;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Services;

public sealed class NotificationLogService : INotificationLogService
{
    private readonly INotificationLogRepository _notificationLogRepository;

    public NotificationLogService(INotificationLogRepository notificationLogRepository)
    {
        _notificationLogRepository = notificationLogRepository;
    }

    public async Task<IReadOnlyList<NotificationLogResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<NotificationLog> notificationLogs = await _notificationLogRepository.GetAllAsync(cancellationToken);
        return notificationLogs.Select(MapToResponse).ToList();
    }

    public async Task<NotificationLogSummaryResponse> GetSummaryAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<NotificationLog> logs = await _notificationLogRepository.GetAllAsync(cancellationToken);

        IReadOnlyDictionary<string, int> byChannel = logs
            .GroupBy(log => log.NotificationType.ToString())
            .ToDictionary(group => group.Key, group => group.Count());

        IReadOnlyDictionary<string, int> byAnnouncementType = logs
            .Where(log => log.Announcement is not null)
            .GroupBy(log => log.Announcement!.AnnouncementType.ToString())
            .ToDictionary(group => group.Key, group => group.Count());

        return new NotificationLogSummaryResponse
        {
            TotalLogs = logs.Count,
            TotalSent = logs.Count(log => log.Status == NotificationStatus.Sent),
            TotalFailed = logs.Count(log => log.Status == NotificationStatus.Failed),
            ByChannel = byChannel,
            ByAnnouncementType = byAnnouncementType
        };
    }

    private static NotificationLogResponse MapToResponse(NotificationLog notificationLog)
    {
        return new NotificationLogResponse
        {
            Id = notificationLog.Id,
            AppUserId = notificationLog.AppUserId,
            RecipientFullName = notificationLog.AppUser?.FullName ?? $"User #{notificationLog.AppUserId}",
            RecipientUserType = notificationLog.AppUser?.UserType ?? default,
            AnnouncementId = notificationLog.AnnouncementId,
            AnnouncementTitle = notificationLog.Announcement?.Title ?? $"Announcement #{notificationLog.AnnouncementId}",
            AnnouncementType = notificationLog.Announcement?.AnnouncementType ?? default,
            NotificationType = notificationLog.NotificationType,
            Status = notificationLog.Status,
            Message = notificationLog.Message,
            ErrorMessage = notificationLog.ErrorMessage,
            SentAt = notificationLog.SentAt
        };
    }
}
