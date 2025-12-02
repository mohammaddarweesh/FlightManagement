using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Flights.Queries.Common;

/// <summary>
/// Data transfer object for Flight entity.
/// </summary>
public record FlightDto(
    Guid Id,
    string FlightNumber,
    string AirlineName,
    string AirlineCode,
    string AircraftModel,
    string DepartureAirportCode,
    string DepartureAirportName,
    string DepartureCity,
    string ArrivalAirportCode,
    string ArrivalAirportName,
    string ArrivalCity,
    DateTime ScheduledDepartureTime,
    DateTime ScheduledArrivalTime,
    DateTime? ActualDepartureTime,
    DateTime? ActualArrivalTime,
    TimeSpan Duration,
    FlightStatus Status,
    string? DepartureTerminal,
    string? DepartureGate,
    string? ArrivalTerminal,
    string? ArrivalGate,
    List<FlightPricingDto> Pricing
);

/// <summary>
/// Pricing information per cabin class.
/// </summary>
public record FlightPricingDto(
    FlightClass CabinClass,
    decimal BasePrice,
    decimal CurrentPrice,
    decimal TaxAmount,
    decimal TotalPrice,
    string Currency,
    int TotalSeats,
    int AvailableSeats
);

