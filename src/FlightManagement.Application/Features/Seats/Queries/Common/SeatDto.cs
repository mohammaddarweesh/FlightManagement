using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Seats.Queries.Common;

/// <summary>
/// Data transfer object for seat information.
/// </summary>
public record SeatDto(
    Guid FlightSeatId,
    Guid SeatId,
    string SeatNumber,
    int Row,
    char Column,
    FlightClass CabinClass,
    SeatType SeatType,
    SeatStatus Status,
    bool IsEmergencyExit,
    bool HasExtraLegroom,
    decimal? Price,
    bool IsAvailable
);

/// <summary>
/// Seat map organized by cabin class.
/// </summary>
public record SeatMapDto(
    Guid FlightId,
    string FlightNumber,
    List<CabinSeatMapDto> Cabins
);

/// <summary>
/// Seats grouped by cabin class.
/// </summary>
public record CabinSeatMapDto(
    FlightClass CabinClass,
    string SeatLayout,
    int TotalSeats,
    int AvailableSeats,
    decimal BasePrice,
    List<SeatRowDto> Rows
);

/// <summary>
/// Seats in a single row.
/// </summary>
public record SeatRowDto(
    int RowNumber,
    List<SeatDto> Seats
);

