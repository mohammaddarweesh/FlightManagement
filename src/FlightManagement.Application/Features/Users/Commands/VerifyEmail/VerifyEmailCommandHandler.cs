using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Users.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(IUnitOfWork unitOfWork, ILogger<VerifyEmailCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Verifying email with token");

        var repository = _unitOfWork.Repository<User>();

        var user = await repository.FirstOrDefaultAsync(
            u => u.EmailVerificationToken == request.Token, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Email verification failed: Invalid token");
            return Result.Failure("Invalid verification token");
        }

        if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
        {
            _logger.LogWarning("Email verification failed: Token expired for user {Id}", user.Id);
            return Result.Failure("Verification token has expired");
        }

        if (user.IsEmailVerified)
        {
            return Result.Success("Email already verified");
        }

        user.IsEmailVerified = true;
        user.EmailVerificationToken = null;
        user.EmailVerificationTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        repository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Email verified successfully for user {Id}", user.Id);
        return Result.Success("Email verified successfully");
    }
}

