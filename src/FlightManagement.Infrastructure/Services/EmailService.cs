using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Notifications.Abstractions;
using FlightManagement.Notifications.Configuration;
using FlightManagement.Notifications.Email.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightManagement.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly INotificationHandler<EmailVerificationNotification> _emailVerificationHandler;
    private readonly INotificationHandler<PasswordResetNotification> _passwordResetHandler;
    private readonly INotificationHandler<WelcomeNotification> _welcomeHandler;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        INotificationHandler<EmailVerificationNotification> emailVerificationHandler,
        INotificationHandler<PasswordResetNotification> passwordResetHandler,
        INotificationHandler<WelcomeNotification> welcomeHandler,
        IOptions<EmailSettings> emailSettings,
        ILogger<EmailService> logger)
    {
        _emailVerificationHandler = emailVerificationHandler;
        _passwordResetHandler = passwordResetHandler;
        _welcomeHandler = welcomeHandler;
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailVerificationAsync(string email, string verificationToken, CancellationToken cancellationToken = default)
    {
        var verificationUrl = $"{_emailSettings.BaseUrl}{_emailSettings.VerifyEmailPath}?token={verificationToken}";

        var notification = new EmailVerificationNotification
        {
            Recipient = email,
            VerificationToken = verificationToken,
            VerificationUrl = verificationUrl
        };

        await _emailVerificationHandler.SendAsync(notification, cancellationToken);

        _logger.LogInformation("Email verification sent to {Email}", email);
    }

    public async Task SendPasswordResetAsync(string email, string resetToken, CancellationToken cancellationToken = default)
    {
        var resetUrl = $"{_emailSettings.BaseUrl}{_emailSettings.ResetPasswordPath}?token={resetToken}";

        var notification = new PasswordResetNotification
        {
            Recipient = email,
            ResetToken = resetToken,
            ResetUrl = resetUrl
        };

        await _passwordResetHandler.SendAsync(notification, cancellationToken);

        _logger.LogInformation("Password reset email sent to {Email}", email);
    }

    public async Task SendWelcomeEmailAsync(string email, string customerName, CancellationToken cancellationToken = default)
    {
        var notification = new WelcomeNotification
        {
            Recipient = email,
            CustomerName = customerName
        };

        await _welcomeHandler.SendAsync(notification, cancellationToken);

        _logger.LogInformation("Welcome email sent to {Email} for customer {CustomerName}", email, customerName);
    }
}

