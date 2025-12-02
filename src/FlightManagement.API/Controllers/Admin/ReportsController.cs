using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Reports.Queries.GetBookingStatusSummary;
using FlightManagement.Application.Features.Reports.Queries.GetPassengerDemographics;
using FlightManagement.Application.Features.Reports.Queries.GetRevenueReport;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for accessing reports and analytics.
/// All endpoints require Admin or Staff role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdminOrStaff]
public class ReportsController : BaseApiController
{
    /// <summary>
    /// Get revenue report by date range with optional filtering.
    /// </summary>
    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] Guid? airlineId = null,
        [FromQuery] Guid? departureAirportId = null,
        [FromQuery] Guid? arrivalAirportId = null,
        [FromQuery] FlightClass? cabinClass = null,
        [FromQuery] ReportGroupBy groupBy = ReportGroupBy.Day)
    {
        var result = await Mediator.Send(new GetRevenueReportQuery(
            startDate,
            endDate,
            airlineId,
            departureAirportId,
            arrivalAirportId,
            cabinClass,
            groupBy
        ));
        return HandleResult(result);
    }

    /// <summary>
    /// Get booking status summary (confirmed, cancelled, no-shows).
    /// </summary>
    [HttpGet("booking-status")]
    public async Task<IActionResult> GetBookingStatusSummary(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] Guid? airlineId = null)
    {
        var result = await Mediator.Send(new GetBookingStatusSummaryQuery(
            startDate,
            endDate,
            airlineId
        ));
        return HandleResult(result);
    }

    /// <summary>
    /// Get passenger demographics and booking pattern insights.
    /// </summary>
    [HttpGet("passenger-demographics")]
    public async Task<IActionResult> GetPassengerDemographics(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] Guid? airlineId = null)
    {
        var result = await Mediator.Send(new GetPassengerDemographicsQuery(
            startDate,
            endDate,
            airlineId
        ));
        return HandleResult(result);
    }
}

