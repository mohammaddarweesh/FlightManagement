using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmPassword
) : ICommand;

