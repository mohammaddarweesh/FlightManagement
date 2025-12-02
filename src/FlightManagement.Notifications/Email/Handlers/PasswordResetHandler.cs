using FlightManagement.Notifications.Abstractions;
using FlightManagement.Notifications.Configuration;
using FlightManagement.Notifications.Email.Notifications;
using FlightManagement.Notifications.Email.Templates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightManagement.Notifications.Email.Handlers;

/// <summary>
/// Handler for sending password reset notifications
/// </summary>
public class PasswordResetHandler : INotificationHandler<PasswordResetNotification>
{
    private readonly IEmailSender _emailSender;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<PasswordResetHandler> _logger;

    public PasswordResetHandler(
        IEmailSender emailSender,
        IOptions<EmailSettings> emailSettings,
        ILogger<PasswordResetHandler> logger)
    {
        _emailSender = emailSender;
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendAsync(PasswordResetNotification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending password reset email to {Email}", notification.Recipient);

        var htmlBody = EmailTemplates.GetPasswordResetHtml(
            notification.ResetUrl,
            _emailSettings.CompanyName);

        var textBody = EmailTemplates.GetPasswordResetText(
            notification.ResetUrl,
            _emailSettings.CompanyName);

        var message = new EmailMessage
        {
            To = notification.Recipient,
            Subject = notification.Subject,
            HtmlBody = htmlBody,
            TextBody = textBody
        };

        await _emailSender.SendEmailAsync(message, cancellationToken);
        
        _logger.LogInformation("Password reset email sent successfully to {Email}", notification.Recipient);
    }
}

