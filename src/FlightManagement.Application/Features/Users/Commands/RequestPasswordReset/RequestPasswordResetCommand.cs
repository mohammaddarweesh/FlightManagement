using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Commands.RequestPasswordReset;

public record RequestPasswordResetCommand(string Email) : ICommand;

