namespace SmartCampusAnnouncement.Application.Models.Observers;

public sealed record ObserverNotificationResult
{
    public int TargetedUserCount { get; init; }

    public int SentCount { get; init; }

    public int FailedCount { get; init; }

    public static ObserverNotificationResult Empty => new();

    public static ObserverNotificationResult Create(int targetedUserCount, int sentCount, int failedCount)
    {
        if (targetedUserCount < 0)
            throw new ArgumentOutOfRangeException(nameof(targetedUserCount));

        if (sentCount < 0)
            throw new ArgumentOutOfRangeException(nameof(sentCount));

        if (failedCount < 0)
            throw new ArgumentOutOfRangeException(nameof(failedCount));

        return new ObserverNotificationResult
        {
            TargetedUserCount = targetedUserCount,
            SentCount = sentCount,
            FailedCount = failedCount
        };
    }
}
