using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;

