using Microsoft.AspNetCore.Mvc;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.DTOs.NotificationLogs;

namespace SmartCampusAnnouncement.WebAPI.Controllers;

[ApiController]
[Route("api/notification-logs")]
public sealed class NotificationLogsController : ControllerBase
{
    private readonly INotificationLogService _notificationLogService;

    public NotificationLogsController(INotificationLogService notificationLogService)
    {
        _notificationLogService = notificationLogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<NotificationLogResponse> logs = await _notificationLogService.GetAllAsync(cancellationToken);
        return Ok(logs);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        NotificationLogSummaryResponse summary = await _notificationLogService.GetSummaryAsync(cancellationToken);
        return Ok(summary);
    }
}
