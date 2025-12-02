using FlightManagement.Notifications.Abstractions;

namespace FlightManagement.Notifications.Email.Notifications;

/// <summary>
/// Password reset notification
/// </summary>
public class PasswordResetNotification : IEmailNotification
{
    public required string Recipient { get; init; }
    public required string ResetToken { get; init; }
    public required string ResetUrl { get; init; }
    
    public string Subject => "Reset Your Password - Flight Management";
    public string? HtmlBody { get; set; }
    public string? TextBody { get; set; }
    public IReadOnlyList<EmailAttachment>? Attachments => null;
}

