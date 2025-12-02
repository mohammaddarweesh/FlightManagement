using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Policies.Queries.GetBookingPolicies;

public record GetBookingPoliciesQuery(
    PolicyType? Type = null,
    bool? IsActive = null,
    Guid? AirlineId = null,
    int Page = 1,
    int PageSize = 20
) : IQuery<BookingPolicyListResult>;

public record BookingPolicyListResult(
    List<BookingPolicyDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record BookingPolicyDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    PolicyType Type,
    int Value,
    string ErrorMessage,
    Guid? AirlineId,
    string? AirlineName,
    Guid? DepartureAirportId,
    string? DepartureAirportCode,
    Guid? ArrivalAirportId,
    string? ArrivalAirportCode,
    FlightClass? CabinClass,
    int Priority,
    bool IsActive
);

