namespace FlightManagement.Notifications.Configuration;

/// <summary>
/// General email settings
/// </summary>
public class EmailSettings
{
    public const string SectionName = "Email";

    public string BaseUrl { get; set; } = "https://localhost:5001";
    public string VerifyEmailPath { get; set; } = "/api/users/verify-email";
    public string ResetPasswordPath { get; set; } = "/reset-password";
    public string CompanyName { get; set; } = "Flight Management";
    public string SupportEmail { get; set; } = "support@flightmanagement.com";
}

