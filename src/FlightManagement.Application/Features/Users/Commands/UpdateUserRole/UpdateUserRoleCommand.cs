using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Users.Commands.UpdateUserRole;

/// <summary>
/// Command to update a user's role.
/// </summary>
public record UpdateUserRoleCommand(
    Guid UserId,
    UserType NewUserType
) : ICommand;

