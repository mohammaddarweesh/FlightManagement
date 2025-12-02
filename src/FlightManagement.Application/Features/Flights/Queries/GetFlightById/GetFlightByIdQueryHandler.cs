using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Flights.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Flights.Queries.GetFlightById;

/// <summary>
/// Handler for getting a flight by ID.
/// </summary>
public class GetFlightByIdQueryHandler : IQueryHandler<GetFlightByIdQuery, FlightDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFlightByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<FlightDto>> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Flight>();

        var flight = await repository.FirstOrDefaultAsync(
            f => f.Id == request.Id,
            cancellationToken,
            f => f.Airline,
            f => f.Aircraft,
            f => f.DepartureAirport,
            f => f.ArrivalAirport,
            f => f.FlightPricings
        );

        if (flight == null)
        {
            return Result<FlightDto>.Failure($"Flight with ID '{request.Id}' not found");
        }

        var dto = new FlightDto(
            flight.Id,
            flight.FlightNumber,
            flight.Airline.Name,
            flight.Airline.IataCode,
            flight.Aircraft.Model,
            flight.DepartureAirport.IataCode,
            flight.DepartureAirport.Name,
            flight.DepartureAirport.City,
            flight.ArrivalAirport.IataCode,
            flight.ArrivalAirport.Name,
            flight.ArrivalAirport.City,
            flight.ScheduledDepartureTime,
            flight.ScheduledArrivalTime,
            flight.ActualDepartureTime,
            flight.ActualArrivalTime,
            flight.Duration,
            flight.Status,
            flight.DepartureTerminal,
            flight.DepartureGate,
            flight.ArrivalTerminal,
            flight.ArrivalGate,
            flight.FlightPricings.Select(p => new FlightPricingDto(
                p.CabinClass,
                p.BasePrice,
                p.CurrentPrice,
                p.TaxAmount,
                p.TotalPrice,
                p.Currency,
                p.TotalSeats,
                p.AvailableSeats
            )).ToList()
        );

        return Result<FlightDto>.Success(dto);
    }
}

