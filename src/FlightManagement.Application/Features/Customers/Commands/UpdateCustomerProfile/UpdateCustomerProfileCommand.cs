using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Customers.Commands.UpdateCustomerProfile;

public record UpdateCustomerProfileCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string? PhoneNumber = null,
    DateTime? DateOfBirth = null,
    string? AddressLine1 = null,
    string? AddressLine2 = null,
    string? City = null,
    string? State = null,
    string? PostalCode = null,
    string? Country = null,
    string? PreferredLanguage = null,
    string? PreferredCurrency = null,
    bool ReceivePromotionalEmails = false,
    bool ReceiveSmsNotifications = false
) : ICommand;

