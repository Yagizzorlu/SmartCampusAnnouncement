using SmartCampusAnnouncement.Application.DTOs.Announcements;

namespace SmartCampusAnnouncement.Application.Abstractions.Services;

public interface IAnnouncementService
{
    Task<IReadOnlyList<AnnouncementResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AnnouncementResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<AnnouncementResponse> CreateAsync(CreateAnnouncementRequest request, CancellationToken cancellationToken = default);

    Task<PublishAnnouncementResponse> PublishAsync(int id, CancellationToken cancellationToken = default);

    Task<AnnouncementResponse> ArchiveAsync(int id, CancellationToken cancellationToken = default);
}
