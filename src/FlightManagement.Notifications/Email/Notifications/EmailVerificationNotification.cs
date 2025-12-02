using FlightManagement.Notifications.Abstractions;

namespace FlightManagement.Notifications.Email.Notifications;

/// <summary>
/// Email verification notification
/// </summary>
public class EmailVerificationNotification : IEmailNotification
{
    public required string Recipient { get; init; }
    public required string VerificationToken { get; init; }
    public required string VerificationUrl { get; init; }
    
    public string Subject => "Verify Your Email - Flight Management";
    public string? HtmlBody { get; set; }
    public string? TextBody { get; set; }
    public IReadOnlyList<EmailAttachment>? Attachments => null;
}

