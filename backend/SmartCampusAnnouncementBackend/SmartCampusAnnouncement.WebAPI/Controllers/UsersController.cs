using Microsoft.AspNetCore.Mvc;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.DTOs.Users;

namespace SmartCampusAnnouncement.WebAPI.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        IReadOnlyList<UserResponse> users = await _userService.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        UserResponse user = await _userService.GetByIdAsync(id, cancellationToken);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        UserResponse user = await _userService.CreateAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, user);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken)
    {
        await _userService.DeactivateAsync(id, cancellationToken);
        return NoContent();
    }
}
