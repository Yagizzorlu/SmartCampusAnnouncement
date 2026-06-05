namespace SmartCampusAnnouncement.Application.DTOs.NotificationLogs;

public sealed record NotificationLogSummaryResponse
{
    public int TotalLogs { get; init; }
    public int TotalSent { get; init; }
    public int TotalFailed { get; init; }
    public IReadOnlyDictionary<string, int> ByChannel { get; init; } = new Dictionary<string, int>();
    public IReadOnlyDictionary<string, int> ByAnnouncementType { get; init; } = new Dictionary<string, int>();
}
