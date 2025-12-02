using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Queries.GetAvailableSeats;

/// <summary>
/// Query to get available seats for a flight.
/// </summary>
public record GetAvailableSeatsQuery(
    Guid FlightId,
    FlightClass? CabinClass = null
) : IQuery<FlightSeatMapDto>;

/// <summary>
/// Flight seat map with available seats.
/// </summary>
public record FlightSeatMapDto(
    Guid FlightId,
    string FlightNumber,
    List<SeatRowDto> Rows
);

/// <summary>
/// Seat row information.
/// </summary>
public record SeatRowDto(
    int RowNumber,
    FlightClass CabinClass,
    List<SeatDto> Seats
);

/// <summary>
/// Individual seat information.
/// </summary>
public record SeatDto(
    Guid FlightSeatId,
    string SeatNumber,
    SeatType SeatType,
    SeatStatus Status,
    bool IsAvailable,
    decimal? SeatFee,
    bool IsWindow,
    bool IsAisle,
    bool IsExitRow,
    bool HasExtraLegroom
);

