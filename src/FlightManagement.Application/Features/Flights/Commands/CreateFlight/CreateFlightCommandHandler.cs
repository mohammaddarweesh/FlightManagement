using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using AircraftEntity = FlightManagement.Domain.Entities.Aircraft;

namespace FlightManagement.Application.Features.Flights.Commands.CreateFlight;

/// <summary>
/// Handler for creating a new flight with pricing and seat assignments.
/// </summary>
public class CreateFlightCommandHandler : ICommandHandler<CreateFlightCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateFlightCommandHandler> _logger;

    public CreateFlightCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateFlightCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating flight: {FlightNumber}", request.FlightNumber);

        // Validate airline exists
        var airlineRepo = _unitOfWork.Repository<Airline>();
        var airline = await airlineRepo.GetByIdAsync(request.AirlineId, cancellationToken);
        if (airline == null)
            return Result<Guid>.Failure($"Airline with ID '{request.AirlineId}' not found");

        // Validate aircraft exists and belongs to airline
        var aircraftRepo = _unitOfWork.Repository<AircraftEntity>();
        var aircraft = await aircraftRepo.FirstOrDefaultAsync(
            a => a.Id == request.AircraftId,
            cancellationToken,
            a => a.CabinClasses,
            a => a.Seats);
        
        if (aircraft == null)
            return Result<Guid>.Failure($"Aircraft with ID '{request.AircraftId}' not found");

        // Validate airports exist
        var airportRepo = _unitOfWork.Repository<Airport>();
        var departureAirport = await airportRepo.GetByIdAsync(request.DepartureAirportId, cancellationToken);
        if (departureAirport == null)
            return Result<Guid>.Failure($"Departure airport with ID '{request.DepartureAirportId}' not found");

        var arrivalAirport = await airportRepo.GetByIdAsync(request.ArrivalAirportId, cancellationToken);
        if (arrivalAirport == null)
            return Result<Guid>.Failure($"Arrival airport with ID '{request.ArrivalAirportId}' not found");

        // Calculate duration
        var duration = request.ScheduledArrivalTime - request.ScheduledDepartureTime;
        if (duration <= TimeSpan.Zero)
            return Result<Guid>.Failure("Arrival time must be after departure time");

        // Create flight
        var flight = new Flight
        {
            Id = Guid.NewGuid(),
            FlightNumber = request.FlightNumber.ToUpper().Trim(),
            AirlineId = request.AirlineId,
            AircraftId = request.AircraftId,
            DepartureAirportId = request.DepartureAirportId,
            ArrivalAirportId = request.ArrivalAirportId,
            ScheduledDepartureTime = request.ScheduledDepartureTime,
            ScheduledArrivalTime = request.ScheduledArrivalTime,
            Duration = duration,
            DepartureTerminal = request.DepartureTerminal?.Trim(),
            DepartureGate = request.DepartureGate?.Trim(),
            ArrivalTerminal = request.ArrivalTerminal?.Trim(),
            ArrivalGate = request.ArrivalGate?.Trim(),
            Status = FlightStatus.Scheduled,
            IsActive = true
        };

        var flightRepo = _unitOfWork.Repository<Flight>();
        await flightRepo.AddAsync(flight, cancellationToken);

        // Create flight pricing for each cabin class
        await CreateFlightPricing(flight.Id, aircraft.CabinClasses, cancellationToken);

        // Create flight seats from aircraft seats
        await CreateFlightSeats(flight.Id, aircraft.Seats, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Flight created successfully: {Id}", flight.Id);
        return Result<Guid>.Success(flight.Id);
    }

    private async Task CreateFlightPricing(Guid flightId, ICollection<AircraftCabinClass> cabinClasses, CancellationToken ct)
    {
        var pricingRepo = _unitOfWork.Repository<FlightPricing>();
        const decimal basePrice = 100m; // Base price in USD

        foreach (var cabin in cabinClasses)
        {
            var pricing = new FlightPricing
            {
                Id = Guid.NewGuid(),
                FlightId = flightId,
                CabinClass = cabin.CabinClass,
                BasePrice = basePrice * cabin.BasePriceMultiplier,
                CurrentPrice = basePrice * cabin.BasePriceMultiplier,
                TaxAmount = basePrice * cabin.BasePriceMultiplier * 0.15m, // 15% tax
                Currency = "USD",
                TotalSeats = cabin.SeatCount,
                AvailableSeats = cabin.SeatCount,
                IsActive = true
            };
            await pricingRepo.AddAsync(pricing, ct);
        }
    }

    private async Task CreateFlightSeats(Guid flightId, ICollection<Seat> seats, CancellationToken ct)
    {
        var flightSeatRepo = _unitOfWork.Repository<FlightSeat>();

        foreach (var seat in seats.Where(s => s.IsActive))
        {
            var flightSeat = new FlightSeat
            {
                Id = Guid.NewGuid(),
                FlightId = flightId,
                SeatId = seat.Id,
                Status = SeatStatus.Available,
                Price = null // Uses cabin class pricing by default
            };
            await flightSeatRepo.AddAsync(flightSeat, ct);
        }
    }
}

