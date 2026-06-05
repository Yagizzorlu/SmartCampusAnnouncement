using Microsoft.EntityFrameworkCore;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AppUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Include(user => user.NotificationPreferences)
            .OrderByDescending(user => user.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<AppUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Include(user => user.NotificationPreferences)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<AppUser>> GetActiveUsersByTypeWithPreferencesAsync(UserType userType, CancellationToken cancellationToken = default)
    {
        return await _context.AppUsers
            .Include(user => user.NotificationPreferences)
            .Where(user => user.IsActive && user.UserType == userType)
            .OrderBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(AppUser user, CancellationToken cancellationToken = default)
    {
        await _context.AppUsers.AddAsync(user, cancellationToken);
    }

    public void Update(AppUser user)
    {
        _context.AppUsers.Update(user);
    }

    public void Remove(AppUser user)
    {
        _context.AppUsers.Remove(user);
    }
}
