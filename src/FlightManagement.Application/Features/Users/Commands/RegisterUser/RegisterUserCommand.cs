using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string Password,
    string ConfirmPassword
) : ICommand<Guid>;

