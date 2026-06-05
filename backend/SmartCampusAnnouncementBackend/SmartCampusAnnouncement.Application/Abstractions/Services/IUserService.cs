using SmartCampusAnnouncement.Application.DTOs.Users;

namespace SmartCampusAnnouncement.Application.Abstractions.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    Task DeactivateAsync(int id, CancellationToken cancellationToken = default);
}
