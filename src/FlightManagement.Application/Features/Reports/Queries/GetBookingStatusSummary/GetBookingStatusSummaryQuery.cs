using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Reports.Queries.GetBookingStatusSummary;

/// <summary>
/// Query to get booking status summary (confirmed, cancelled, no-shows).
/// </summary>
public record GetBookingStatusSummaryQuery(
    DateTime StartDate,
    DateTime EndDate,
    Guid? AirlineId = null
) : IQuery<BookingStatusSummaryResult>;

public record BookingStatusSummaryResult(
    DateTime StartDate,
    DateTime EndDate,
    int TotalBookings,
    List<StatusCount> StatusBreakdown,
    List<DailyStatusSummary> DailyTrend
);

public record StatusCount(
    BookingStatus Status,
    int Count,
    decimal Percentage,
    decimal TotalAmount
);

public record DailyStatusSummary(
    DateTime Date,
    int Confirmed,
    int Cancelled,
    int Pending,
    int NoShow,
    int Total
);

