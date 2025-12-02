using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Commands.ActivateUser;

/// <summary>
/// Command to activate a user account.
/// </summary>
public record ActivateUserCommand(Guid UserId) : ICommand;

