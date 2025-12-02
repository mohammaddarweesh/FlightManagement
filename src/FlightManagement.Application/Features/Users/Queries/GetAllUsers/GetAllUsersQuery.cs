using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Users.Queries.GetAllUsers;

/// <summary>
/// Query to get all users with optional filtering.
/// </summary>
public record GetAllUsersQuery(
    UserType? UserType = null,
    bool? IsEmailVerified = null,
    bool? IsActive = null,
    string? SearchTerm = null,
    int Page = 1,
    int PageSize = 20
) : IQuery<PagedUserResult>;

public record PagedUserResult(
    IEnumerable<UserDto> Users,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record UserDto(
    Guid Id,
    string Email,
    UserType UserType,
    bool IsEmailVerified,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastLoginAt
);

