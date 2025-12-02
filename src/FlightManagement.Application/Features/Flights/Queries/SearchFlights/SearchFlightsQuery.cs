using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Flights.Queries.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Flights.Queries.SearchFlights;

/// <summary>
/// Query to search for available flights.
/// This is the main customer-facing search query.
/// </summary>
public record SearchFlightsQuery(
    string? DepartureAirportCode = null,
    string? ArrivalAirportCode = null,
    DateTime? DepartureDate = null,
    FlightClass? CabinClass = null,
    int? MinAvailableSeats = null
) : IQuery<List<FlightDto>>;

