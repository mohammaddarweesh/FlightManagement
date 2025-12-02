using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Flights.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Flights.Queries.SearchFlights;

/// <summary>
/// Handler for searching available flights.
/// </summary>
public class SearchFlightsQueryHandler : IQueryHandler<SearchFlightsQuery, List<FlightDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public SearchFlightsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<FlightDto>>> Handle(SearchFlightsQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Flight>();

        // Get flights with all related data
        var flights = await repository.GetAllAsync(
            f => f.IsActive &&
                 f.Status == FlightStatus.Scheduled &&
                 (string.IsNullOrEmpty(request.DepartureAirportCode) || 
                  f.DepartureAirport.IataCode == request.DepartureAirportCode.ToUpper()) &&
                 (string.IsNullOrEmpty(request.ArrivalAirportCode) || 
                  f.ArrivalAirport.IataCode == request.ArrivalAirportCode.ToUpper()) &&
                 (!request.DepartureDate.HasValue || 
                  f.ScheduledDepartureTime.Date == request.DepartureDate.Value.Date),
            cancellationToken,
            f => f.Airline,
            f => f.Aircraft,
            f => f.DepartureAirport,
            f => f.ArrivalAirport,
            f => f.FlightPricings
        );

        // Filter by cabin class availability if specified
        if (request.CabinClass.HasValue || request.MinAvailableSeats.HasValue)
        {
            flights = flights.Where(f =>
                f.FlightPricings.Any(p =>
                    (!request.CabinClass.HasValue || p.CabinClass == request.CabinClass.Value) &&
                    (!request.MinAvailableSeats.HasValue || p.AvailableSeats >= request.MinAvailableSeats.Value)
                )
            ).ToList();
        }

        var dtos = flights.Select(f => new FlightDto(
            f.Id,
            f.FlightNumber,
            f.Airline.Name,
            f.Airline.IataCode,
            f.Aircraft.Model,
            f.DepartureAirport.IataCode,
            f.DepartureAirport.Name,
            f.DepartureAirport.City,
            f.ArrivalAirport.IataCode,
            f.ArrivalAirport.Name,
            f.ArrivalAirport.City,
            f.ScheduledDepartureTime,
            f.ScheduledArrivalTime,
            f.ActualDepartureTime,
            f.ActualArrivalTime,
            f.Duration,
            f.Status,
            f.DepartureTerminal,
            f.DepartureGate,
            f.ArrivalTerminal,
            f.ArrivalGate,
            f.FlightPricings.Select(p => new FlightPricingDto(
                p.CabinClass,
                p.BasePrice,
                p.CurrentPrice,
                p.TaxAmount,
                p.TotalPrice,
                p.Currency,
                p.TotalSeats,
                p.AvailableSeats
            )).ToList()
        )).OrderBy(f => f.ScheduledDepartureTime).ToList();

        return Result<List<FlightDto>>.Success(dtos);
    }
}

