using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Aircraft.Queries.Common;

/// <summary>
/// Data transfer object for Aircraft entity.
/// </summary>
public record AircraftDto(
    Guid Id,
    Guid AirlineId,
    string AirlineName,
    string Model,
    string Manufacturer,
    string RegistrationNumber,
    int TotalSeats,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    List<CabinClassDto> CabinClasses
);

/// <summary>
/// Cabin class configuration for an aircraft.
/// </summary>
public record CabinClassDto(
    Guid Id,
    FlightClass CabinClass,
    int SeatCount,
    int RowStart,
    int RowEnd,
    string SeatLayout,
    decimal BasePriceMultiplier
);

/// <summary>
/// Simple aircraft DTO without nested data.
/// </summary>
public record AircraftSimpleDto(
    Guid Id,
    string AirlineName,
    string Model,
    string Manufacturer,
    string RegistrationNumber,
    int TotalSeats,
    bool IsActive
);

