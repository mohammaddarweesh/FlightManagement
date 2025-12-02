namespace FlightManagement.Application.Features.Users.Queries.GetUserById;

public record UserDto(
    Guid Id,
    string Email,
    bool IsEmailVerified,
    bool IsActive,
    DateTime? LastLoginAt,
    DateTime CreatedAt
);

