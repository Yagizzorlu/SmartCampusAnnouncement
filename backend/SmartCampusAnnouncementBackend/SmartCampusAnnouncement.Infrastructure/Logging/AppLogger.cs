using SmartCampusAnnouncement.Application.Abstractions.Logging;

namespace SmartCampusAnnouncement.Infrastructure.Logging;

public sealed class AppLogger : IAppLogger
{
    private static readonly Lazy<AppLogger> LazyInstance = new(() => new AppLogger());

    private readonly object _lockObject = new();

    public static AppLogger Instance => LazyInstance.Value;

    private AppLogger()
    {
    }

    public void Information(string message)
    {
        Write("INFO", message, ConsoleColor.Green);
    }

    public void Warning(string message)
    {
        Write("WARN", message, ConsoleColor.Yellow);
    }

    public void Error(string message, Exception? exception = null)
    {
        string finalMessage = exception is null ? message : $"{message} Exception: {exception.Message}";
        Write("ERROR", finalMessage, ConsoleColor.Red);
    }

    private void Write(string level, string message, ConsoleColor color)
    {
        lock (_lockObject)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
            Console.ForegroundColor = previousColor;
        }
    }
}
