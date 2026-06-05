using Microsoft.EntityFrameworkCore;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Infrastructure.Persistence.Repositories;

public sealed class NotificationLogRepository : INotificationLogRepository
{
    private readonly AppDbContext _context;

    public NotificationLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<NotificationLog>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.NotificationLogs
            .Include(log => log.AppUser)
            .Include(log => log.Announcement)
            .OrderByDescending(log => log.SentAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(NotificationLog notificationLog, CancellationToken cancellationToken = default)
    {
        await _context.NotificationLogs.AddAsync(notificationLog, cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<NotificationLog> notificationLogs, CancellationToken cancellationToken = default)
    {
        await _context.NotificationLogs.AddRangeAsync(notificationLogs, cancellationToken);
    }
}
