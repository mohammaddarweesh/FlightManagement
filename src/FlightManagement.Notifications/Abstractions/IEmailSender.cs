namespace FlightManagement.Notifications.Abstractions;

/// <summary>
/// Interface for sending emails
/// </summary>
public interface IEmailSender
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents an email message
/// </summary>
public class EmailMessage
{
    public required string To { get; init; }
    public string? Cc { get; init; }
    public string? Bcc { get; init; }
    public required string Subject { get; init; }
    public string? HtmlBody { get; init; }
    public string? TextBody { get; init; }
    public List<EmailAttachment>? Attachments { get; init; }
    public Dictionary<string, string>? Headers { get; init; }
}

