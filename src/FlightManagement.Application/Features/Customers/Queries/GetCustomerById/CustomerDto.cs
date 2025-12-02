namespace FlightManagement.Application.Features.Customers.Queries.GetCustomerById;

public record CustomerDto(
    Guid Id,
    Guid UserId,
    string FirstName,
    string LastName,
    string FullName,
    string? PhoneNumber,
    DateTime? DateOfBirth,
    AddressDto? Address,
    PreferencesDto Preferences,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record AddressDto(
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? State,
    string? PostalCode,
    string? Country
);

public record PreferencesDto(
    string? PreferredLanguage,
    string? PreferredCurrency,
    bool ReceivePromotionalEmails,
    bool ReceiveSmsNotifications
);

