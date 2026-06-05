using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<IReadOnlyList<AppUser>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AppUser?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AppUser>> GetActiveUsersByTypeWithPreferencesAsync(UserType userType, CancellationToken cancellationToken = default);

    Task AddAsync(AppUser user, CancellationToken cancellationToken = default);

    void Update(AppUser user);

    void Remove(AppUser user);
}
