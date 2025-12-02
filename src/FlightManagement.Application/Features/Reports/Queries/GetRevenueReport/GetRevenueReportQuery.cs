using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Reports.Queries.GetRevenueReport;

/// <summary>
/// Query to get revenue report by date range, routes, and classes.
/// </summary>
public record GetRevenueReportQuery(
    DateTime StartDate,
    DateTime EndDate,
    Guid? AirlineId = null,
    Guid? DepartureAirportId = null,
    Guid? ArrivalAirportId = null,
    FlightClass? CabinClass = null,
    ReportGroupBy GroupBy = ReportGroupBy.Day
) : IQuery<RevenueReportResult>;

public enum ReportGroupBy
{
    Day,
    Week,
    Month,
    Route,
    CabinClass,
    Airline
}

public record RevenueReportResult(
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalRevenue,
    decimal TotalRefunds,
    decimal NetRevenue,
    int TotalBookings,
    int TotalPassengers,
    decimal AverageBookingValue,
    string Currency,
    List<RevenueBreakdown> Breakdown
);

public record RevenueBreakdown(
    string GroupKey,
    string GroupLabel,
    decimal Revenue,
    decimal Refunds,
    decimal NetRevenue,
    int BookingCount,
    int PassengerCount
);

