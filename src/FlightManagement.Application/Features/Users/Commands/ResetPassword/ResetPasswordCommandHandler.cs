using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Users.Commands.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<ResetPasswordCommandHandler> _logger;

    public ResetPasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ILogger<ResetPasswordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Resetting password with token");

        if (request.NewPassword != request.ConfirmPassword)
        {
            return Result.Failure("Passwords do not match");
        }

        var repository = _unitOfWork.Repository<User>();

        var user = await repository.FirstOrDefaultAsync(
            u => u.PasswordResetToken == request.Token, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Password reset failed: Invalid token");
            return Result.Failure("Invalid reset token");
        }

        if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            _logger.LogWarning("Password reset failed: Token expired for user {Id}", user.Id);
            return Result.Failure("Reset token has expired");
        }

        user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;

        repository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Password reset successfully for user {Id}", user.Id);
        return Result.Success("Password reset successfully");
    }
}

