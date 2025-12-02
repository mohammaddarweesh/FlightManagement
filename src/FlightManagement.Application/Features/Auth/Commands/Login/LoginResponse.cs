namespace FlightManagement.Application.Features.Auth.Commands.Login;

public record LoginResponse(
    string AccessToken,
    Guid UserId,
    string Email,
    bool IsEmailVerified,
    DateTime ExpiresAt
);

