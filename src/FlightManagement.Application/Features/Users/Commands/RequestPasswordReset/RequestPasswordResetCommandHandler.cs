using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Users.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandHandler : ICommandHandler<RequestPasswordResetCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly ILogger<RequestPasswordResetCommandHandler> _logger;

    public RequestPasswordResetCommandHandler(
        IUnitOfWork unitOfWork,
        ITokenService tokenService,
        IEmailService emailService,
        ILogger<RequestPasswordResetCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Password reset requested for: {Email}", request.Email);

        var repository = _unitOfWork.Repository<User>();

        var user = await repository.FirstOrDefaultAsync(
            u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

        // Always return success to prevent email enumeration
        if (user == null)
        {
            _logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
            return Result.Success("If a user exists with this email, a password reset link will be sent");
        }

        var resetToken = _tokenService.GeneratePasswordResetToken();
        
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);
        user.UpdatedAt = DateTime.UtcNow;

        repository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _emailService.SendPasswordResetAsync(user.Email, resetToken, cancellationToken);

        _logger.LogInformation("Password reset email sent to: {Email}", user.Email);
        return Result.Success("If a user exists with this email, a password reset link will be sent");
    }
}

