using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Policies.Queries.GetBlackoutDates;

public record GetBlackoutDatesQuery(
    bool? IsActive = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    Guid? AirlineId = null,
    int Page = 1,
    int PageSize = 20
) : IQuery<BlackoutDateListResult>;

public record BlackoutDateListResult(
    List<BlackoutDateDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record BlackoutDateDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime StartDate,
    DateTime EndDate,
    bool BlocksBookings,
    bool BlocksPromotions,
    Guid? AirlineId,
    string? AirlineName,
    Guid? DepartureAirportId,
    string? DepartureAirportCode,
    Guid? ArrivalAirportId,
    string? ArrivalAirportCode,
    bool IsActive
);

