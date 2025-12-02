namespace FlightManagement.Notifications.Abstractions;

/// <summary>
/// Interface for email notifications
/// </summary>
public interface IEmailNotification : INotification
{
    string Subject { get; }
    string? HtmlBody { get; }
    string? TextBody { get; }
    IReadOnlyList<EmailAttachment>? Attachments { get; }
}

/// <summary>
/// Represents an email attachment
/// </summary>
public record EmailAttachment(
    string FileName,
    byte[] Content,
    string ContentType
);

