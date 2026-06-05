using SmartCampusAnnouncement.Application.Abstractions.Factories;
using SmartCampusAnnouncement.Application.DTOs.Announcements;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Factories;

public sealed class AnnouncementFactory : IAnnouncementFactory
{
    private const string DefaultCreatedBy = "System Admin";

    public Announcement Create(CreateAnnouncementRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateRequest(request);

        return new Announcement
        {
            Title = request.Title.Trim(),
            Content = request.Content.Trim(),
            AnnouncementType = request.AnnouncementType,
            TargetAudience = request.TargetAudience,
            Priority = request.Priority ?? GetDefaultPriority(request.AnnouncementType),
            Status = AnnouncementStatus.Draft,
            ExpiresAt = request.ExpiresAt,
            CreatedBy = string.IsNullOrWhiteSpace(request.CreatedBy)
                ? DefaultCreatedBy
                : request.CreatedBy.Trim()
        };
    }

    private static void ValidateRequest(CreateAnnouncementRequest request)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Title);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Content);

        if (!Enum.IsDefined(request.AnnouncementType))
            throw new ArgumentOutOfRangeException(nameof(request.AnnouncementType), "Invalid announcement type.");

        if (!Enum.IsDefined(request.TargetAudience))
            throw new ArgumentOutOfRangeException(nameof(request.TargetAudience), "Invalid target audience.");

        if (request.Priority.HasValue && !Enum.IsDefined(request.Priority.Value))
            throw new ArgumentOutOfRangeException(nameof(request.Priority), "Invalid announcement priority.");

        if (request.ExpiresAt.HasValue && request.ExpiresAt.Value <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be a future date.", nameof(request.ExpiresAt));
    }

    private static AnnouncementPriority GetDefaultPriority(AnnouncementType announcementType)
    {
        return announcementType switch
        {
            AnnouncementType.Exam => AnnouncementPriority.Urgent,
            AnnouncementType.Event => AnnouncementPriority.Normal,
            AnnouncementType.FoodMenu => AnnouncementPriority.Normal,
            AnnouncementType.Library => AnnouncementPriority.High,
            _ => AnnouncementPriority.Normal
        };
    }
}
