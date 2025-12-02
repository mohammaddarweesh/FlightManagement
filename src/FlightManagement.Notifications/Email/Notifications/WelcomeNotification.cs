using FlightManagement.Notifications.Abstractions;

namespace FlightManagement.Notifications.Email.Notifications;

/// <summary>
/// Welcome notification for new customers
/// </summary>
public class WelcomeNotification : IEmailNotification
{
    public required string Recipient { get; init; }
    public required string CustomerName { get; init; }
    
    public string Subject => "Welcome to Flight Management!";
    public string? HtmlBody { get; set; }
    public string? TextBody { get; set; }
    public IReadOnlyList<EmailAttachment>? Attachments => null;
}

