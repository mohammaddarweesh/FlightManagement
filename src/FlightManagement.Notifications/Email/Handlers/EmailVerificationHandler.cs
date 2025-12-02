using FlightManagement.Notifications.Abstractions;
using FlightManagement.Notifications.Configuration;
using FlightManagement.Notifications.Email.Notifications;
using FlightManagement.Notifications.Email.Templates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightManagement.Notifications.Email.Handlers;

/// <summary>
/// Handler for sending email verification notifications
/// </summary>
public class EmailVerificationHandler : INotificationHandler<EmailVerificationNotification>
{
    private readonly IEmailSender _emailSender;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailVerificationHandler> _logger;

    public EmailVerificationHandler(
        IEmailSender emailSender,
        IOptions<EmailSettings> emailSettings,
        ILogger<EmailVerificationHandler> logger)
    {
        _emailSender = emailSender;
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendAsync(EmailVerificationNotification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending email verification to {Email}", notification.Recipient);

        var htmlBody = EmailTemplates.GetEmailVerificationHtml(
            notification.VerificationUrl,
            _emailSettings.CompanyName);

        var textBody = EmailTemplates.GetEmailVerificationText(
            notification.VerificationUrl,
            _emailSettings.CompanyName);

        var message = new EmailMessage
        {
            To = notification.Recipient,
            Subject = notification.Subject,
            HtmlBody = htmlBody,
            TextBody = textBody
        };

        await _emailSender.SendEmailAsync(message, cancellationToken);
        
        _logger.LogInformation("Email verification sent successfully to {Email}", notification.Recipient);
    }
}

