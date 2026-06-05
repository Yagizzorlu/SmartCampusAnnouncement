using SmartCampusAnnouncement.Application.Abstractions.Time;

namespace SmartCampusAnnouncement.Infrastructure.Time;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
