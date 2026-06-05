using SmartCampusAnnouncement.Application.Abstractions.Factories;
using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Application.Abstractions.Publishing;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.Abstractions.Time;
using SmartCampusAnnouncement.Application.DTOs.Announcements;
using SmartCampusAnnouncement.Application.Exceptions;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Services;

public sealed class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;
    private readonly IAnnouncementFactory _announcementFactory;
    private readonly IAnnouncementPublisher _announcementPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAppLogger _logger;

    public AnnouncementService(IAnnouncementRepository announcementRepository, IAnnouncementFactory announcementFactory, IAnnouncementPublisher announcementPublisher, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IAppLogger logger)
    {
        _announcementRepository = announcementRepository;
        _announcementFactory = announcementFactory;
        _announcementPublisher = announcementPublisher;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<IReadOnlyList<AnnouncementResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Announcement> announcements = await _announcementRepository.GetAllAsync(cancellationToken);
        return announcements.Select(MapToResponse).ToList();
    }

    public async Task<AnnouncementResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Announcement announcement = await _announcementRepository.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For<Announcement>(id);

        return MapToResponse(announcement);
    }

    public async Task<AnnouncementResponse> CreateAsync(CreateAnnouncementRequest request, CancellationToken cancellationToken = default)
    {
        Announcement announcement = _announcementFactory.Create(request);

        await _announcementRepository.AddAsync(announcement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Announcement '{announcement.Title}' created as '{announcement.Status}'.");

        return MapToResponse(announcement);
    }

    public async Task<PublishAnnouncementResponse> PublishAsync(int id, CancellationToken cancellationToken = default)
    {
        Announcement announcement = await _announcementRepository.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For<Announcement>(id);

        EnsureAnnouncementCanBePublished(announcement);

        announcement.Status = AnnouncementStatus.Published;
        announcement.PublishedAt = _dateTimeProvider.UtcNow;

        _announcementRepository.Update(announcement);

        PublishAnnouncementResponse response = await _announcementPublisher.PublishAsync(announcement, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Announcement '{announcement.Title}' published. Sent: {response.SentCount}, Failed: {response.FailedCount}.");

        return response;
    }

    public async Task<AnnouncementResponse> ArchiveAsync(int id, CancellationToken cancellationToken = default)
    {
        Announcement announcement = await _announcementRepository.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For<Announcement>(id);

        if (announcement.Status == AnnouncementStatus.Archived)
        {
            _logger.Warning($"Announcement '{announcement.Title}' is already archived.");
            return MapToResponse(announcement);
        }

        announcement.Status = AnnouncementStatus.Archived;

        _announcementRepository.Update(announcement);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"Announcement '{announcement.Title}' archived.");

        return MapToResponse(announcement);
    }

    private void EnsureAnnouncementCanBePublished(Announcement announcement)
    {
        if (announcement.Status == AnnouncementStatus.Published)
            throw new BusinessRuleException("Announcement has already been published.");

        if (announcement.Status == AnnouncementStatus.Archived)
            throw new BusinessRuleException("Archived announcements cannot be published.");

        if (announcement.ExpiresAt.HasValue && announcement.ExpiresAt.Value <= _dateTimeProvider.UtcNow)
            throw new BusinessRuleException("Expired announcements cannot be published.");
    }

    private static AnnouncementResponse MapToResponse(Announcement announcement)
    {
        return new AnnouncementResponse
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            AnnouncementType = announcement.AnnouncementType,
            TargetAudience = announcement.TargetAudience,
            Status = announcement.Status,
            Priority = announcement.Priority,
            PublishedAt = announcement.PublishedAt,
            ExpiresAt = announcement.ExpiresAt,
            CreatedBy = announcement.CreatedBy,
            CreatedAt = announcement.CreatedAt
        };
    }
}
