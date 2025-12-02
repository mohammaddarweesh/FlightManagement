using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Reports.Queries.GetPassengerDemographics;

/// <summary>
/// Query to get passenger demographic and booking pattern insights.
/// </summary>
public record GetPassengerDemographicsQuery(
    DateTime StartDate,
    DateTime EndDate,
    Guid? AirlineId = null
) : IQuery<PassengerDemographicsResult>;

public record PassengerDemographicsResult(
    DateTime StartDate,
    DateTime EndDate,
    int TotalPassengers,
    int UniqueCustomers,
    decimal AveragePassengersPerBooking,
    List<PassengerTypeBreakdown> PassengerTypes,
    List<CabinClassPreference> CabinClassPreferences,
    List<BookingPatternInsight> BookingPatterns,
    List<TopRoute> TopRoutes,
    List<FrequentCustomer> FrequentCustomers
);

public record PassengerTypeBreakdown(
    PassengerType Type,
    int Count,
    decimal Percentage
);

public record CabinClassPreference(
    FlightClass CabinClass,
    int BookingCount,
    decimal Percentage,
    decimal AverageSpend
);

public record BookingPatternInsight(
    string Pattern,
    string Description,
    int Count,
    decimal Percentage
);

public record TopRoute(
    string DepartureAirportCode,
    string ArrivalAirportCode,
    int BookingCount,
    int PassengerCount,
    decimal TotalRevenue
);

public record FrequentCustomer(
    Guid CustomerId,
    string CustomerName,
    string Email,
    int BookingCount,
    int TotalFlights,
    decimal TotalSpent
);

