using System.Security.Claims;
using FlightManagement.API.Authorization;
using FlightManagement.API.Models.Requests;
using FlightManagement.Application.Features.Customers.Commands.CreateCustomerProfile;
using FlightManagement.Application.Features.Customers.Commands.UpdateCustomerProfile;
using FlightManagement.Application.Features.Customers.Queries.GetCustomerByUserId;
using FlightManagement.Application.Features.Customers.Queries.GetCustomerById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers;

[Authorize]
public class CustomersController : BaseApiController
{
    /// <summary>
    /// Create customer profile for current user
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateCustomerProfileRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var command = new CreateCustomerProfileCommand(
            userId.Value,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.DateOfBirth,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.PreferredLanguage,
            request.PreferredCurrency,
            request.ReceivePromotionalEmails,
            request.ReceiveSmsNotifications
        );

        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Get current user's customer profile
    /// </summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var result = await Mediator.Send(new GetCustomerByUserIdQuery(userId.Value));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Get customer by id (Admin only)
    /// </summary>
    [HttpGet("{id:guid}")]
    [RequireAdmin]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetCustomerByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Update customer profile
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateCustomerProfileRequest request)
    {
        var command = new UpdateCustomerProfileCommand(
            id,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.DateOfBirth,
            request.AddressLine1,
            request.AddressLine2,
            request.City,
            request.State,
            request.PostalCode,
            request.Country,
            request.PreferredLanguage,
            request.PreferredCurrency,
            request.ReceivePromotionalEmails,
            request.ReceiveSmsNotifications
        );

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}

