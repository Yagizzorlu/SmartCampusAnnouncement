namespace SmartCampusAnnouncement.Application.Models.Notifications;

public sealed record NotificationDeliveryResult
{
    private NotificationDeliveryResult(bool isSuccess, string message, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Message = message;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }

    public string Message { get; }

    public string? ErrorMessage { get; }

    public static NotificationDeliveryResult Success(string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        return new NotificationDeliveryResult(isSuccess: true, message: message, errorMessage: null);
    }

    public static NotificationDeliveryResult Failed(string message, string errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        ArgumentException.ThrowIfNullOrWhiteSpace(errorMessage);
        return new NotificationDeliveryResult(isSuccess: false, message: message, errorMessage: errorMessage);
    }
}
