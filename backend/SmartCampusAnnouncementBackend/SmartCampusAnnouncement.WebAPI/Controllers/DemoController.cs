using Microsoft.AspNetCore.Mvc;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.DTOs.Announcements;
using SmartCampusAnnouncement.Application.DTOs.NotificationLogs;
using SmartCampusAnnouncement.Application.DTOs.Users;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.WebAPI.Controllers;

[ApiController]
[Route("api/demo")]
public sealed class DemoController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAnnouncementService _announcementService;
    private readonly INotificationLogService _notificationLogService;

    public DemoController(IUserService userService, IAnnouncementService announcementService, INotificationLogService notificationLogService)
    {
        _userService = userService;
        _announcementService = announcementService;
        _notificationLogService = notificationLogService;
    }

    [HttpPost("run-scenario")]
    public async Task<IActionResult> RunScenario(CancellationToken cancellationToken)
    {
        string suffix = DateTime.UtcNow.Ticks.ToString()[^8..];

        UserResponse student = await _userService.CreateAsync(new CreateUserRequest
        {
            FullName = $"Demo Student {suffix}",
            Email = $"student.{suffix}@demo.com",
            PhoneNumber = "5551112233",
            UserType = UserType.Student,
            NotificationTypes = new[] { NotificationType.Email, NotificationType.Push }
        }, cancellationToken);

        UserResponse teacher = await _userService.CreateAsync(new CreateUserRequest
        {
            FullName = $"Demo Teacher {suffix}",
            Email = $"teacher.{suffix}@demo.com",
            PhoneNumber = "5554445566",
            UserType = UserType.Teacher,
            NotificationTypes = new[] { NotificationType.Email, NotificationType.Sms }
        }, cancellationToken);

        AnnouncementResponse announcement = await _announcementService.CreateAsync(new CreateAnnouncementRequest
        {
            Title = $"Final Sinav Duyurusu [{suffix}]",
            Content = "Yazilim Mimarisi ve Tasarimi final sinavi tarihine iliskin guncelleme yapilmistir.",
            AnnouncementType = AnnouncementType.Exam,
            TargetAudience = TargetAudience.All,
            Priority = AnnouncementPriority.Urgent,
            ExpiresAt = DateTime.UtcNow.AddMonths(6),
            CreatedBy = "System Admin"
        }, cancellationToken);

        PublishAnnouncementResponse publishResult = await _announcementService.PublishAsync(announcement.Id, cancellationToken);

        IReadOnlyList<NotificationLogResponse> allLogs = await _notificationLogService.GetAllAsync(cancellationToken);
        IReadOnlyList<NotificationLogResponse> scenarioLogs = allLogs
            .Where(log => log.AnnouncementId == announcement.Id)
            .ToList();

        return Ok(new
        {
            scenario = "SmartCampus Announcement Demo",
            patterns = new
            {
                factory = "AnnouncementNotificationModelFactory produced ExamAnnouncement for type Exam",
                observer = "StudentAnnouncementObserver + TeacherAnnouncementObserver notified",
                singleton = "AppLogger.Instance used throughout the flow"
            },
            usersCreated = new[] { student, teacher },
            announcementCreated = announcement,
            publishResult,
            notificationLogs = scenarioLogs
        });
    }
}
