using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Token,
    string NewPassword,
    string ConfirmPassword
) : ICommand;

