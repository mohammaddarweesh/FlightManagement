using FlightManagement.Domain.Entities;

namespace FlightManagement.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string GenerateEmailVerificationToken();
    string GeneratePasswordResetToken();
}

