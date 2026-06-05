using Microsoft.Extensions.DependencyInjection;
using SmartCampusAnnouncement.Application.Abstractions.Factories;
using SmartCampusAnnouncement.Application.Abstractions.Notifications;
using SmartCampusAnnouncement.Application.Abstractions.Observers;
using SmartCampusAnnouncement.Application.Abstractions.Publishing;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.Factories;
using SmartCampusAnnouncement.Application.Observers;
using SmartCampusAnnouncement.Application.Publishers;
using SmartCampusAnnouncement.Application.Services;

namespace SmartCampusAnnouncement.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnnouncementFactory, AnnouncementFactory>();
        services.AddScoped<INotificationChannelFactory, NotificationChannelFactory>();
        services.AddScoped<IAnnouncementNotificationModelFactory, AnnouncementNotificationModelFactory>();

        services.AddScoped<IAnnouncementObserver, StudentAnnouncementObserver>();
        services.AddScoped<IAnnouncementObserver, TeacherAnnouncementObserver>();
        services.AddScoped<IAnnouncementPublisher, AnnouncementPublisher>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        services.AddScoped<INotificationLogService, NotificationLogService>();

        return services;
    }
}
