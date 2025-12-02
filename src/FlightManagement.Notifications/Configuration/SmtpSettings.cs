namespace FlightManagement.Notifications.Configuration;

/// <summary>
/// SMTP configuration settings
/// </summary>
public class SmtpSettings
{
    public const string SectionName = "Smtp";

    public string Host { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Flight Management";
    public bool UseSsl { get; set; } = true;
    public bool UseStartTls { get; set; } = true;
    public int TimeoutMs { get; set; } = 30000;
}

