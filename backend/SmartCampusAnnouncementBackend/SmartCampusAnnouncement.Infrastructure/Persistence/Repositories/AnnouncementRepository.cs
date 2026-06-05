using Microsoft.EntityFrameworkCore;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Infrastructure.Persistence.Repositories;

public sealed class AnnouncementRepository : IAnnouncementRepository
{
    private readonly AppDbContext _context;

    public AnnouncementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Announcement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Announcements
            .OrderByDescending(announcement => announcement.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Announcement?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Announcements
            .FirstOrDefaultAsync(announcement => announcement.Id == id, cancellationToken);
    }

    public async Task AddAsync(Announcement announcement, CancellationToken cancellationToken = default)
    {
        await _context.Announcements.AddAsync(announcement, cancellationToken);
    }

    public void Update(Announcement announcement)
    {
        _context.Announcements.Update(announcement);
    }
}
