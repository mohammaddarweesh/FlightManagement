using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Queries.SearchAvailableFlights;

/// <summary>
/// Query to search for available flights.
/// </summary>
public record SearchAvailableFlightsQuery(
    string DepartureAirportCode,
    string ArrivalAirportCode,
    DateTime DepartureDate,
    FlightClass? CabinClass = null,
    int PassengerCount = 1,
    DateTime? ReturnDate = null
) : IQuery<FlightSearchResult>;

/// <summary>
/// Result of flight search.
/// </summary>
public record FlightSearchResult(
    List<FlightAvailabilityDto> OutboundFlights,
    List<FlightAvailabilityDto>? ReturnFlights
);

/// <summary>
/// Flight availability information.
/// </summary>
public record FlightAvailabilityDto(
    Guid FlightId,
    string FlightNumber,
    string AirlineName,
    string AirlineCode,
    string DepartureAirportCode,
    string DepartureAirportName,
    string ArrivalAirportCode,
    string ArrivalAirportName,
    DateTime ScheduledDepartureTime,
    DateTime ScheduledArrivalTime,
    TimeSpan Duration,
    string? DepartureTerminal,
    string? DepartureGate,
    List<CabinClassAvailabilityDto> CabinClasses
);

/// <summary>
/// Cabin class availability and pricing.
/// </summary>
public record CabinClassAvailabilityDto(
    FlightClass CabinClass,
    int AvailableSeats,
    decimal BasePrice,
    decimal TaxAmount,
    decimal TotalPrice,
    string Currency
);

