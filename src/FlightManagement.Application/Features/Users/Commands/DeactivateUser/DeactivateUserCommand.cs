using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Commands.DeactivateUser;

/// <summary>
/// Command to deactivate a user account.
/// </summary>
public record DeactivateUserCommand(Guid UserId) : ICommand;

