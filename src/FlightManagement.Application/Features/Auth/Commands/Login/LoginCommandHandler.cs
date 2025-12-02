using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IConfiguration configuration,
        ILogger<LoginCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for: {Email}", request.Email);

        var repository = _unitOfWork.Repository<User>();

        var user = await repository.FirstOrDefaultAsync(
            u => u.Email.ToLower() == request.Email.ToLower(), cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found for {Email}", request.Email);
            return Result<LoginResponse>.Failure("Invalid email or password");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed: User is inactive {Email}", request.Email);
            return Result<LoginResponse>.Failure("User is inactive");
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: Invalid password for {Email}", request.Email);
            return Result<LoginResponse>.Failure("Invalid email or password");
        }

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        repository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Generate token
        var accessToken = _tokenService.GenerateAccessToken(user);
        var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60");

        var response = new LoginResponse(
            accessToken,
            user.Id,
            user.Email,
            user.IsEmailVerified,
            DateTime.UtcNow.AddMinutes(expireMinutes)
        );

        _logger.LogInformation("Login successful for: {Email}", request.Email);
        return Result<LoginResponse>.Success(response);
    }
}

