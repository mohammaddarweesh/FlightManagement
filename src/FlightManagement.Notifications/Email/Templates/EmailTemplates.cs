namespace FlightManagement.Notifications.Email.Templates;

/// <summary>
/// Static class containing email templates
/// </summary>
public static class EmailTemplates
{
    public static string GetEmailVerificationHtml(string verificationUrl, string companyName)
    {
        return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Verify Your Email</title>
            </head>
            <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                    <h1 style="color: white; margin: 0;">✈️ {companyName}</h1>
                </div>
                <div style="background: #ffffff; padding: 30px; border: 1px solid #e0e0e0; border-top: none; border-radius: 0 0 10px 10px;">
                    <h2 style="color: #333;">Verify Your Email Address</h2>
                    <p>Thank you for registering with {companyName}! Please click the button below to verify your email address.</p>
                    <div style="text-align: center; margin: 30px 0;">
                        <a href="{verificationUrl}"
                           style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                                  color: white;
                                  padding: 15px 30px;
                                  text-decoration: none;
                                  border-radius: 5px;
                                  font-weight: bold;
                                  display: inline-block;">
                            Verify Email
                        </a>
                    </div>
                    <p style="color: #666; font-size: 14px;">Or copy and paste this link into your browser:</p>
                    <p style="color: #667eea; word-break: break-all; font-size: 14px;">{verificationUrl}</p>
                    <p style="color: #666; font-size: 14px;">This link will expire in 24 hours.</p>
                    <hr style="border: none; border-top: 1px solid #e0e0e0; margin: 30px 0;">
                    <p style="color: #999; font-size: 12px; text-align: center;">
                        If you didn't create an account, you can safely ignore this email.
                    </p>
                </div>
            </body>
            </html>
            """;
    }

    public static string GetEmailVerificationText(string verificationUrl, string companyName)
    {
        return $"""
            {companyName} - Verify Your Email Address

            Thank you for registering with {companyName}!

            Please click the link below to verify your email address:
            {verificationUrl}

            This link will expire in 24 hours.

            If you didn't create an account, you can safely ignore this email.
            """;
    }

    public static string GetPasswordResetHtml(string resetUrl, string companyName)
    {
        return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Reset Your Password</title>
            </head>
            <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                    <h1 style="color: white; margin: 0;">✈️ {companyName}</h1>
                </div>
                <div style="background: #ffffff; padding: 30px; border: 1px solid #e0e0e0; border-top: none; border-radius: 0 0 10px 10px;">
                    <h2 style="color: #333;">Reset Your Password</h2>
                    <p>We received a request to reset your password. Click the button below to create a new password.</p>
                    <div style="text-align: center; margin: 30px 0;">
                        <a href="{resetUrl}"
                           style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                                  color: white;
                                  padding: 15px 30px;
                                  text-decoration: none;
                                  border-radius: 5px;
                                  font-weight: bold;
                                  display: inline-block;">
                            Reset Password
                        </a>
                    </div>
                    <p style="color: #666; font-size: 14px;">Or copy and paste this link:</p>
                    <p style="color: #667eea; word-break: break-all; font-size: 14px;">{resetUrl}</p>
                    <p style="color: #666; font-size: 14px;">This link will expire in 1 hour.</p>
                    <hr style="border: none; border-top: 1px solid #e0e0e0; margin: 30px 0;">
                    <p style="color: #999; font-size: 12px; text-align: center;">
                        If you didn't request a password reset, you can safely ignore this email.
                    </p>
                </div>
            </body>
            </html>
            """;
    }

    public static string GetPasswordResetText(string resetUrl, string companyName)
    {
        return $"""
            {companyName} - Reset Your Password

            We received a request to reset your password.

            Click the link below to create a new password:
            {resetUrl}

            This link will expire in 1 hour.

            If you didn't request a password reset, you can safely ignore this email.
            """;
    }

    public static string GetWelcomeHtml(string customerName, string companyName, string supportEmail)
    {
        return $"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Welcome to {companyName}</title>
            </head>
            <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px;">
                <div style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center; border-radius: 10px 10px 0 0;">
                    <h1 style="color: white; margin: 0;">✈️ {companyName}</h1>
                </div>
                <div style="background: #ffffff; padding: 30px; border: 1px solid #e0e0e0; border-top: none; border-radius: 0 0 10px 10px;">
                    <h2 style="color: #333;">Welcome, {customerName}! 🎉</h2>
                    <p>Thank you for creating your profile with {companyName}. We're excited to have you on board!</p>
                    <p>Here's what you can do now:</p>
                    <ul style="color: #666;">
                        <li>🔍 Search for flights to your favorite destinations</li>
                        <li>📅 Book and manage your reservations</li>
                        <li>💺 Select your preferred seats</li>
                        <li>🧳 Add baggage and special services</li>
                    </ul>
                    <p>If you have any questions, feel free to reach out to our support team at <a href="mailto:{supportEmail}" style="color: #667eea;">{supportEmail}</a>.</p>
                    <p>Happy travels! ✈️</p>
                    <hr style="border: none; border-top: 1px solid #e0e0e0; margin: 30px 0;">
                    <p style="color: #999; font-size: 12px; text-align: center;">
                        © {DateTime.UtcNow.Year} {companyName}. All rights reserved.
                    </p>
                </div>
            </body>
            </html>
            """;
    }

    public static string GetWelcomeText(string customerName, string companyName, string supportEmail)
    {
        return $"""
            Welcome to {companyName}, {customerName}!

            Thank you for creating your profile. We're excited to have you on board!

            Here's what you can do now:
            - Search for flights to your favorite destinations
            - Book and manage your reservations
            - Select your preferred seats
            - Add baggage and special services

            If you have any questions, feel free to reach out to our support team at {supportEmail}.

            Happy travels!

            © {DateTime.UtcNow.Year} {companyName}. All rights reserved.
            """;
    }
}
