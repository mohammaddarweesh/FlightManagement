using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IEmailService emailService,
        ILogger<RegisterUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registering new user for email: {Email}", request.Email);

        if (request.Password != request.ConfirmPassword)
        {
            return Result<Guid>.Failure("Passwords do not match");
        }

        var repository = _unitOfWork.Repository<User>();

        var existingUser = await repository.FirstOrDefaultAsync(
            u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: Email {Email} already exists", request.Email);
            return Result<Guid>.Failure("A user with this email already exists");
        }

        var verificationToken = _tokenService.GenerateEmailVerificationToken();
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email.ToLower().Trim(),
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            IsEmailVerified = false,
            EmailVerificationToken = verificationToken,
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await repository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _emailService.SendEmailVerificationAsync(user.Email, verificationToken, cancellationToken);

        _logger.LogInformation("User registered successfully: {Id}", user.Id);
        return Result<Guid>.Success(user.Id);
    }
}

