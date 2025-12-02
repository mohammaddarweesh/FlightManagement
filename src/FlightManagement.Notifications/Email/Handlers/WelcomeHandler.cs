using FlightManagement.Notifications.Abstractions;
using FlightManagement.Notifications.Configuration;
using FlightManagement.Notifications.Email.Notifications;
using FlightManagement.Notifications.Email.Templates;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FlightManagement.Notifications.Email.Handlers;

/// <summary>
/// Handler for sending welcome notifications
/// </summary>
public class WelcomeHandler : INotificationHandler<WelcomeNotification>
{
    private readonly IEmailSender _emailSender;
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<WelcomeHandler> _logger;

    public WelcomeHandler(
        IEmailSender emailSender,
        IOptions<EmailSettings> emailSettings,
        ILogger<WelcomeHandler> logger)
    {
        _emailSender = emailSender;
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendAsync(WelcomeNotification notification, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending welcome email to {Email} for {CustomerName}", 
            notification.Recipient, notification.CustomerName);

        var htmlBody = EmailTemplates.GetWelcomeHtml(
            notification.CustomerName,
            _emailSettings.CompanyName,
            _emailSettings.SupportEmail);

        var textBody = EmailTemplates.GetWelcomeText(
            notification.CustomerName,
            _emailSettings.CompanyName,
            _emailSettings.SupportEmail);

        var message = new EmailMessage
        {
            To = notification.Recipient,
            Subject = notification.Subject,
            HtmlBody = htmlBody,
            TextBody = textBody
        };

        await _emailSender.SendEmailAsync(message, cancellationToken);
        
        _logger.LogInformation("Welcome email sent successfully to {Email}", notification.Recipient);
    }
}

