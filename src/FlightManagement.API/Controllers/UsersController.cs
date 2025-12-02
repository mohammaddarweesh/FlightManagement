using System.Security.Claims;
using FlightManagement.API.Authorization;
using FlightManagement.API.Models.Requests;
using FlightManagement.Application.Features.Users.Commands.ChangePassword;
using FlightManagement.Application.Features.Users.Commands.RegisterUser;
using FlightManagement.Application.Features.Users.Commands.RequestPasswordReset;
using FlightManagement.Application.Features.Users.Commands.ResetPassword;
using FlightManagement.Application.Features.Users.Commands.VerifyEmail;
using FlightManagement.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers;

public class UsersController : BaseApiController
{
    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Verify email with token
    /// </summary>
    [HttpGet("verify-email")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token)
    {
        var result = await Mediator.Send(new VerifyEmailCommand(token));
        return HandleResult(result);
    }

    /// <summary>
    /// Request password reset
    /// </summary>
    [HttpPost("request-password-reset")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Reset password with token
    /// </summary>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Change password (requires authentication)
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var command = new ChangePasswordCommand(
            userId.Value,
            request.CurrentPassword,
            request.NewPassword,
            request.ConfirmPassword
        );

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Get current user
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var result = await Mediator.Send(new GetUserByIdQuery(userId.Value));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Get user by id (Admin only)
    /// </summary>
    [HttpGet("{id:guid}")]
    [RequireAdmin]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}

