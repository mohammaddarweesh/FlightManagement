using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using AircraftEntity = FlightManagement.Domain.Entities.Aircraft;

namespace FlightManagement.Application.Features.Aircraft.Commands.CreateAircraft;

/// <summary>
/// Handler for creating a new aircraft with cabin classes and seats.
/// </summary>
public class CreateAircraftCommandHandler : ICommandHandler<CreateAircraftCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAircraftCommandHandler> _logger;

    public CreateAircraftCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAircraftCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateAircraftCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating aircraft: {Model} - {Registration}", request.Model, request.RegistrationNumber);

        // Validate airline exists
        var airlineRepo = _unitOfWork.Repository<Airline>();
        var airline = await airlineRepo.GetByIdAsync(request.AirlineId, cancellationToken);
        if (airline == null)
        {
            return Result<Guid>.Failure($"Airline with ID '{request.AirlineId}' not found");
        }

        // Check for duplicate registration
        var aircraftRepo = _unitOfWork.Repository<AircraftEntity>();
        var existingByReg = await aircraftRepo.FirstOrDefaultAsync(
            a => a.RegistrationNumber == request.RegistrationNumber.ToUpper(),
            cancellationToken);
        if (existingByReg != null)
        {
            return Result<Guid>.Failure($"Aircraft with registration '{request.RegistrationNumber}' already exists");
        }

        // Calculate total seats from cabin classes
        int totalSeats = 0;
        var cabinClasses = new List<AircraftCabinClass>();
        var seats = new List<Seat>();

        foreach (var cabinInput in request.CabinClasses)
        {
            int seatsPerRow = cabinInput.SeatLayout.Replace("-", "").Length;
            int rows = cabinInput.RowEnd - cabinInput.RowStart + 1;
            int seatCount = rows * seatsPerRow;
            totalSeats += seatCount;

            var cabinClass = new AircraftCabinClass
            {
                CabinClass = cabinInput.CabinClass,
                SeatCount = seatCount,
                RowStart = cabinInput.RowStart,
                RowEnd = cabinInput.RowEnd,
                SeatLayout = cabinInput.SeatLayout,
                BasePriceMultiplier = cabinInput.BasePriceMultiplier
            };
            cabinClasses.Add(cabinClass);

            // Generate seats
            var seatColumns = cabinInput.SeatLayout.Replace("-", "");
            for (int row = cabinInput.RowStart; row <= cabinInput.RowEnd; row++)
            {
                for (int colIdx = 0; colIdx < seatColumns.Length; colIdx++)
                {
                    char column = seatColumns[colIdx];
                    seats.Add(new Seat
                    {
                        SeatNumber = $"{row}{column}",
                        Row = row,
                        Column = column,
                        CabinClass = cabinInput.CabinClass,
                        SeatType = GetSeatType(colIdx, seatColumns.Length),
                        IsEmergencyExit = false,
                        HasExtraLegroom = false,
                        IsActive = true
                    });
                }
            }
        }

        var aircraft = new AircraftEntity
        {
            AirlineId = request.AirlineId,
            Model = request.Model,
            Manufacturer = request.Manufacturer,
            RegistrationNumber = request.RegistrationNumber.ToUpper(),
            TotalSeats = totalSeats,
            IsActive = true,
            CabinClasses = cabinClasses,
            Seats = seats
        };

        await aircraftRepo.AddAsync(aircraft, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Aircraft created: {AircraftId} with {SeatCount} seats", aircraft.Id, totalSeats);
        return Result<Guid>.Success(aircraft.Id);
    }

    private static SeatType GetSeatType(int columnIndex, int totalColumns)
    {
        if (columnIndex == 0 || columnIndex == totalColumns - 1)
            return SeatType.Window;
        if (columnIndex == 1 || columnIndex == totalColumns - 2)
            return SeatType.Middle;
        return SeatType.Aisle;
    }
}

