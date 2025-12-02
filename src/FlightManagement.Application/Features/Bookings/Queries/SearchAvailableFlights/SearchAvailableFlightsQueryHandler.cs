using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Queries.SearchAvailableFlights;

/// <summary>
/// Handler for searching available flights.
/// </summary>
public class SearchAvailableFlightsQueryHandler : IQueryHandler<SearchAvailableFlightsQuery, FlightSearchResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SearchAvailableFlightsQueryHandler> _logger;

    public SearchAvailableFlightsQueryHandler(IUnitOfWork unitOfWork, ILogger<SearchAvailableFlightsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FlightSearchResult>> Handle(SearchAvailableFlightsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching flights from {From} to {To} on {Date}", 
            request.DepartureAirportCode, request.ArrivalAirportCode, request.DepartureDate);

        // Get airports
        var airportRepo = _unitOfWork.Repository<Airport>();
        var departureAirport = await airportRepo.FirstOrDefaultAsync(
            a => a.IataCode == request.DepartureAirportCode.ToUpper(), cancellationToken);
        var arrivalAirport = await airportRepo.FirstOrDefaultAsync(
            a => a.IataCode == request.ArrivalAirportCode.ToUpper(), cancellationToken);

        if (departureAirport == null)
            return Result<FlightSearchResult>.Failure($"Departure airport '{request.DepartureAirportCode}' not found");
        if (arrivalAirport == null)
            return Result<FlightSearchResult>.Failure($"Arrival airport '{request.ArrivalAirportCode}' not found");

        // Search outbound flights
        var outboundFlights = await SearchFlights(
            departureAirport.Id, arrivalAirport.Id, request.DepartureDate, 
            request.CabinClass, request.PassengerCount, cancellationToken);

        // Search return flights if requested
        List<FlightAvailabilityDto>? returnFlights = null;
        if (request.ReturnDate.HasValue)
        {
            returnFlights = await SearchFlights(
                arrivalAirport.Id, departureAirport.Id, request.ReturnDate.Value,
                request.CabinClass, request.PassengerCount, cancellationToken);
        }

        return Result<FlightSearchResult>.Success(new FlightSearchResult(outboundFlights, returnFlights));
    }

    private async Task<List<FlightAvailabilityDto>> SearchFlights(
        Guid departureAirportId, Guid arrivalAirportId, DateTime date,
        FlightClass? cabinClass, int passengerCount, CancellationToken cancellationToken)
    {
        var flightRepo = _unitOfWork.Repository<Flight>();
        var startOfDay = date.Date;
        var endOfDay = date.Date.AddDays(1);

        var flights = await flightRepo.Query()
            .Include(f => f.Airline)
            .Include(f => f.DepartureAirport)
            .Include(f => f.ArrivalAirport)
            .Include(f => f.FlightPricings)
            .Where(f => f.DepartureAirportId == departureAirportId
                && f.ArrivalAirportId == arrivalAirportId
                && f.ScheduledDepartureTime >= startOfDay
                && f.ScheduledDepartureTime < endOfDay
                && f.IsActive
                && f.Status == FlightStatus.Scheduled)
            .OrderBy(f => f.ScheduledDepartureTime)
            .ToListAsync(cancellationToken);

        var result = new List<FlightAvailabilityDto>();

        foreach (var flight in flights)
        {
            var cabinClasses = flight.FlightPricings
                .Where(p => p.IsActive && p.AvailableSeats >= passengerCount)
                .Where(p => !cabinClass.HasValue || p.CabinClass == cabinClass.Value)
                .Select(p => new CabinClassAvailabilityDto(
                    p.CabinClass,
                    p.AvailableSeats,
                    p.CurrentPrice,
                    p.TaxAmount,
                    p.CurrentPrice + p.TaxAmount,
                    p.Currency
                ))
                .ToList();

            if (cabinClasses.Count > 0)
            {
                result.Add(new FlightAvailabilityDto(
                    flight.Id,
                    flight.FlightNumber,
                    flight.Airline?.Name ?? "",
                    flight.Airline?.IataCode ?? "",
                    flight.DepartureAirport?.IataCode ?? "",
                    flight.DepartureAirport?.Name ?? "",
                    flight.ArrivalAirport?.IataCode ?? "",
                    flight.ArrivalAirport?.Name ?? "",
                    flight.ScheduledDepartureTime,
                    flight.ScheduledArrivalTime,
                    flight.ScheduledArrivalTime - flight.ScheduledDepartureTime,
                    flight.DepartureTerminal,
                    flight.DepartureGate,
                    cabinClasses
                ));
            }
        }

        return result;
    }
}

