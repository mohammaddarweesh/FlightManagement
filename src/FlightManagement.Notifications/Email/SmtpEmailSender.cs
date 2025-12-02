using FlightManagement.Notifications.Abstractions;
using FlightManagement.Notifications.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FlightManagement.Notifications.Email;

/// <summary>
/// SMTP email sender using MailKit
/// </summary>
public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IOptions<SmtpSettings> settings, ILogger<SmtpEmailSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var mimeMessage = CreateMimeMessage(message);

        using var client = new SmtpClient();
        
        try
        {
            _logger.LogInformation("Connecting to SMTP server {Host}:{Port}", _settings.Host, _settings.Port);
            
            var secureSocketOptions = _settings.UseStartTls 
                ? SecureSocketOptions.StartTls 
                : (_settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None);

            await client.ConnectAsync(_settings.Host, _settings.Port, secureSocketOptions, cancellationToken);
            
            if (!string.IsNullOrEmpty(_settings.Username))
            {
                await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            }

            await client.SendAsync(mimeMessage, cancellationToken);
            
            _logger.LogInformation("Email sent successfully to {To}", message.To);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", message.To);
            throw;
        }
        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }

    private MimeMessage CreateMimeMessage(EmailMessage message)
    {
        var mimeMessage = new MimeMessage();
        
        mimeMessage.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        mimeMessage.To.Add(MailboxAddress.Parse(message.To));
        
        if (!string.IsNullOrEmpty(message.Cc))
        {
            mimeMessage.Cc.Add(MailboxAddress.Parse(message.Cc));
        }
        
        if (!string.IsNullOrEmpty(message.Bcc))
        {
            mimeMessage.Bcc.Add(MailboxAddress.Parse(message.Bcc));
        }
        
        mimeMessage.Subject = message.Subject;

        var builder = new BodyBuilder();
        
        if (!string.IsNullOrEmpty(message.HtmlBody))
        {
            builder.HtmlBody = message.HtmlBody;
        }
        
        if (!string.IsNullOrEmpty(message.TextBody))
        {
            builder.TextBody = message.TextBody;
        }

        if (message.Attachments != null)
        {
            foreach (var attachment in message.Attachments)
            {
                builder.Attachments.Add(attachment.FileName, attachment.Content, ContentType.Parse(attachment.ContentType));
            }
        }

        mimeMessage.Body = builder.ToMessageBody();

        if (message.Headers != null)
        {
            foreach (var header in message.Headers)
            {
                mimeMessage.Headers.Add(header.Key, header.Value);
            }
        }

        return mimeMessage;
    }
}

