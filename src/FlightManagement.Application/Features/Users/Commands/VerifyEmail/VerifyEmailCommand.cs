using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Commands.VerifyEmail;

public record VerifyEmailCommand(string Token) : ICommand;

