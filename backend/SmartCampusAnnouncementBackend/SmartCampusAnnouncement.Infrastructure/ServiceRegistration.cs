using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Notifications;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Application.Abstractions.Time;
using SmartCampusAnnouncement.Infrastructure.Logging;
using SmartCampusAnnouncement.Infrastructure.Notifications;
using SmartCampusAnnouncement.Infrastructure.Persistence;
using SmartCampusAnnouncement.Infrastructure.Persistence.Repositories;
using SmartCampusAnnouncement.Infrastructure.Time;

namespace SmartCampusAnnouncement.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
        services.AddScoped<INotificationLogRepository, NotificationLogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IAppLogger>(_ => AppLogger.Instance);
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<INotificationChannel, EmailNotificationChannel>();
        services.AddScoped<INotificationChannel, SmsNotificationChannel>();
        services.AddScoped<INotificationChannel, PushNotificationChannel>();

        return services;
    }
}
