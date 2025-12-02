namespace FlightManagement.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailVerificationAsync(string email, string verificationToken, CancellationToken cancellationToken = default);
    Task SendPasswordResetAsync(string email, string resetToken, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(string email, string customerName, CancellationToken cancellationToken = default);
}

