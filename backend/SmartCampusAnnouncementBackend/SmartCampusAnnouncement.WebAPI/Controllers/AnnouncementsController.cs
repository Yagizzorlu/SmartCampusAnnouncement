using Microsoft.AspNetCore.Mvc;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.DTOs.Announcements;

namespace SmartCampusAnnouncement.WebAPI.Controllers;

[ApiController]
[Route("api/announcements")]
public sealed class AnnouncementsController : ControllerBase
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementsController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<AnnouncementResponse> announcements = await _announcementService.GetAllAsync(cancellationToken);
        return Ok(announcements);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        AnnouncementResponse announcement = await _announcementService.GetByIdAsync(id, cancellationToken);
        return Ok(announcement);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnnouncementRequest request, CancellationToken cancellationToken)
    {
        AnnouncementResponse announcement = await _announcementService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, announcement);
    }

    [HttpPost("{id:int}/publish")]
    public async Task<IActionResult> Publish(int id, CancellationToken cancellationToken)
    {
        PublishAnnouncementResponse response = await _announcementService.PublishAsync(id, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{id:int}/archive")]
    public async Task<IActionResult> Archive(int id, CancellationToken cancellationToken)
    {
        AnnouncementResponse announcement = await _announcementService.ArchiveAsync(id, cancellationToken);
        return Ok(announcement);
    }
}
