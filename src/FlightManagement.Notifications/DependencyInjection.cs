using FlightManagement.Notifications.Abstractions;
using FlightManagement.Notifications.Configuration;
using FlightManagement.Notifications.Email;
using FlightManagement.Notifications.Email.Handlers;
using FlightManagement.Notifications.Email.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightManagement.Notifications;

/// <summary>
/// Dependency injection extensions for the Notifications library
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure settings
        services.Configure<SmtpSettings>(configuration.GetSection(SmtpSettings.SectionName));
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

        // Register email sender
        services.AddScoped<IEmailSender, SmtpEmailSender>();

        // Register notification handlers
        services.AddScoped<INotificationHandler<EmailVerificationNotification>, EmailVerificationHandler>();
        services.AddScoped<INotificationHandler<PasswordResetNotification>, PasswordResetHandler>();
        services.AddScoped<INotificationHandler<WelcomeNotification>, WelcomeHandler>();

        return services;
    }
}

