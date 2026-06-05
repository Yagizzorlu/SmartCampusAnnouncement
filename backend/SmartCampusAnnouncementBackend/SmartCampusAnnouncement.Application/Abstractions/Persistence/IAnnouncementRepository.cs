using SmartCampusAnnouncement.Domain.Entities;

namespace SmartCampusAnnouncement.Application.Abstractions.Persistence;

public interface IAnnouncementRepository
{
    Task<IReadOnlyList<Announcement>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Announcement?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task AddAsync(Announcement announcement, CancellationToken cancellationToken = default);

    void Update(Announcement announcement);
}
