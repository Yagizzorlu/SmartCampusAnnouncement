using System.Net.Mail;
using SmartCampusAnnouncement.Application.Abstractions.Logging;
using SmartCampusAnnouncement.Application.Abstractions.Persistence;
using SmartCampusAnnouncement.Application.Abstractions.Services;
using SmartCampusAnnouncement.Application.DTOs.Users;
using SmartCampusAnnouncement.Application.Exceptions;
using SmartCampusAnnouncement.Domain.Entities;
using SmartCampusAnnouncement.Domain.Enums;

namespace SmartCampusAnnouncement.Application.Services;

public sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger _logger;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IAppLogger logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<AppUser> users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(MapToResponse).ToList();
    }

    public async Task<UserResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        AppUser user = await _userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For<AppUser>(id);

        return MapToResponse(user);
    }

    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateCreateRequest(request);

        List<NotificationType> distinctNotificationTypes = request.NotificationTypes.Distinct().ToList();

        AppUser user = new()
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim(),
            UserType = request.UserType,
            IsActive = true
        };

        foreach (NotificationType notificationType in distinctNotificationTypes)
        {
            user.NotificationPreferences.Add(new NotificationPreference
            {
                NotificationType = notificationType
            });
        }

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"User '{user.FullName}' created with {distinctNotificationTypes.Count} notification preference(s).");

        return MapToResponse(user);
    }

    public async Task DeactivateAsync(int id, CancellationToken cancellationToken = default)
    {
        AppUser user = await _userRepository.GetByIdAsync(id, cancellationToken)
            ?? throw NotFoundException.For<AppUser>(id);

        if (!user.IsActive)
        {
            _logger.Warning($"User '{user.FullName}' is already inactive.");
            return;
        }

        user.IsActive = false;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Information($"User '{user.FullName}' deactivated.");
    }

    private static void ValidateCreateRequest(CreateUserRequest request)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.FullName);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Email);

        if (!IsValidEmail(request.Email))
            throw new BusinessRuleException("Email address is not valid.");

        if (!Enum.IsDefined(request.UserType))
            throw new BusinessRuleException("User type is not valid.");

        if (request.NotificationTypes.Count == 0)
            throw new BusinessRuleException("At least one notification preference must be selected.");

        foreach (NotificationType notificationType in request.NotificationTypes)
        {
            if (!Enum.IsDefined(notificationType))
                throw new BusinessRuleException($"Notification type '{notificationType}' is not valid.");
        }

        bool smsSelected = request.NotificationTypes.Contains(NotificationType.Sms);

        if (smsSelected && string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new BusinessRuleException("Phone number is required when SMS notification is selected.");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static UserResponse MapToResponse(AppUser user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserType = user.UserType,
            IsActive = user.IsActive,
            NotificationTypes = user.NotificationPreferences
                .Select(preference => preference.NotificationType)
                .Distinct()
                .ToArray(),
            CreatedAt = user.CreatedAt
        };
    }
}
