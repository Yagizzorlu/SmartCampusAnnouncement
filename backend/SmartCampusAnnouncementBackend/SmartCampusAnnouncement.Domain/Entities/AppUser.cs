using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Domain.Entities;

public class AppUser : BaseEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public UserType UserType { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<NotificationPreference> NotificationPreferences { get; set; } = new List<NotificationPreference>();

    public ICollection<NotificationLog> NotificationLogs { get; set; } = new List<NotificationLog>();
}
