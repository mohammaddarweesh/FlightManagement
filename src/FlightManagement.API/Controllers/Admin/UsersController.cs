using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Users.Commands.ActivateUser;
using FlightManagement.Application.Features.Users.Commands.DeactivateUser;
using FlightManagement.Application.Features.Users.Commands.UpdateUserRole;
using FlightManagement.Application.Features.Users.Queries.GetAllUsers;
using FlightManagement.Application.Features.Users.Queries.GetUserById;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

[RequireAdmin]
[Route("api/admin/users")]
public class UsersController : BaseApiController
{
    /// <summary>
    /// Get all users with optional filtering and pagination.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] UserType? userType,
        [FromQuery] bool? isEmailVerified,
        [FromQuery] bool? isActive,
        [FromQuery] string? searchTerm,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await Mediator.Send(new GetAllUsersQuery(
            userType,
            isEmailVerified,
            isActive,
            searchTerm,
            page,
            pageSize
        ));
        return HandleResult(result);
    }

    /// <summary>
    /// Get user by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Update a user's role.
    /// </summary>
    [HttpPut("{id:guid}/role")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleRequest request)
    {
        var result = await Mediator.Send(new UpdateUserRoleCommand(id, request.NewUserType));
        return HandleResult(result);
    }

    /// <summary>
    /// Deactivate a user account.
    /// </summary>
    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await Mediator.Send(new DeactivateUserCommand(id));
        return HandleResult(result);
    }

    /// <summary>
    /// Activate a user account.
    /// </summary>
    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var result = await Mediator.Send(new ActivateUserCommand(id));
        return HandleResult(result);
    }
}

public record UpdateRoleRequest(UserType NewUserType);

