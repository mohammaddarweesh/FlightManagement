using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Seats.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Seats.Queries.GetFlightSeatMap;

/// <summary>
/// Handler for getting the seat map for a flight.
/// </summary>
public class GetFlightSeatMapQueryHandler : IQueryHandler<GetFlightSeatMapQuery, SeatMapDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFlightSeatMapQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SeatMapDto>> Handle(GetFlightSeatMapQuery request, CancellationToken cancellationToken)
    {
        var flightRepo = _unitOfWork.Repository<Flight>();
        var flight = await flightRepo.FirstOrDefaultAsync(
            f => f.Id == request.FlightId,
            cancellationToken,
            f => f.Aircraft.CabinClasses,
            f => f.FlightPricings
        );

        if (flight == null)
        {
            return Result<SeatMapDto>.Failure($"Flight with ID '{request.FlightId}' not found");
        }

        var flightSeatRepo = _unitOfWork.Repository<FlightSeat>();
        var flightSeats = await flightSeatRepo.GetAllAsync(
            fs => fs.FlightId == request.FlightId,
            cancellationToken,
            fs => fs.Seat
        );

        // Group seats by cabin class
        var cabins = new List<CabinSeatMapDto>();

        foreach (var cabinClass in flight.Aircraft.CabinClasses.OrderBy(c => c.CabinClass))
        {
            var cabinSeats = flightSeats
                .Where(fs => fs.Seat.CabinClass == cabinClass.CabinClass)
                .ToList();

            var pricing = flight.FlightPricings.FirstOrDefault(p => p.CabinClass == cabinClass.CabinClass);

            var rows = cabinSeats
                .GroupBy(fs => fs.Seat.Row)
                .OrderBy(g => g.Key)
                .Select(g => new SeatRowDto(
                    g.Key,
                    g.OrderBy(fs => fs.Seat.Column)
                     .Select(fs => new SeatDto(
                         fs.Id,
                         fs.SeatId,
                         fs.Seat.SeatNumber,
                         fs.Seat.Row,
                         fs.Seat.Column,
                         fs.Seat.CabinClass,
                         fs.Seat.SeatType,
                         fs.Status,
                         fs.Seat.IsEmergencyExit,
                         fs.Seat.HasExtraLegroom,
                         fs.Price ?? pricing?.CurrentPrice,
                         fs.CanBeReserved
                     )).ToList()
                )).ToList();

            cabins.Add(new CabinSeatMapDto(
                cabinClass.CabinClass,
                cabinClass.SeatLayout,
                cabinSeats.Count,
                cabinSeats.Count(s => s.CanBeReserved),
                pricing?.CurrentPrice ?? 0,
                rows
            ));
        }

        var seatMap = new SeatMapDto(
            flight.Id,
            flight.FlightNumber,
            cabins
        );

        return Result<SeatMapDto>.Success(seatMap);
    }
}

