using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Commands.CreateBooking;

/// <summary>
/// Handler for creating a new booking.
/// </summary>
public class CreateBookingCommandHandler : ICommandHandler<CreateBookingCommand, CreateBookingResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBookingCommandHandler> _logger;

    public CreateBookingCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateBookingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CreateBookingResult>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating booking for customer: {CustomerId}", request.CustomerId);

        // Validate customer exists
        var customerRepo = _unitOfWork.Repository<Customer>();
        var customer = await customerRepo.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            return Result<CreateBookingResult>.Failure($"Customer with ID '{request.CustomerId}' not found");

        // Validate and get flights with pricing
        var validationResult = await ValidateSegmentsAndCalculatePricing(request.Segments, request.Passengers.Count, cancellationToken);
        if (!validationResult.IsSuccess)
            return Result<CreateBookingResult>.Failure(validationResult.Errors.FirstOrDefault() ?? "Validation failed");

        var (segments, baseFare, taxAmount) = validationResult.Data;

        // Calculate fees
        decimal serviceFee = 10.00m; // Flat service fee - could be configurable
        decimal totalAmount = baseFare + taxAmount + serviceFee;

        // Create booking
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            BookingReference = GenerateBookingReference(),
            CustomerId = request.CustomerId,
            Status = BookingStatus.Pending,
            BookingDate = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30), // 30-minute payment window
            TripType = request.TripType,
            BaseFare = baseFare,
            TaxAmount = taxAmount,
            ServiceFee = serviceFee,
            SeatSelectionFees = 0,
            ExtrasFees = 0,
            DiscountAmount = 0,
            TotalAmount = totalAmount,
            Currency = "USD",
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            SpecialRequests = request.SpecialRequests,
            PaymentStatus = PaymentStatus.Pending,
            PaidAmount = 0,
            PaymentDueDate = DateTime.UtcNow.AddMinutes(30),
            CancellationPolicyId = request.CancellationPolicyId,
            PromoCode = request.PromoCode
        };

        var bookingRepo = _unitOfWork.Repository<Booking>();
        await bookingRepo.AddAsync(booking, cancellationToken);

        // Create booking segments
        var segmentRepo = _unitOfWork.Repository<BookingSegment>();
        foreach (var segment in segments)
        {
            segment.BookingId = booking.Id;
            await segmentRepo.AddAsync(segment, cancellationToken);
        }

        // Create passengers
        var passengerRepo = _unitOfWork.Repository<Passenger>();
        foreach (var passengerInput in request.Passengers)
        {
            var passenger = CreatePassenger(booking.Id, passengerInput);
            await passengerRepo.AddAsync(passenger, cancellationToken);
        }

        // Add booking history entry
        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Action = BookingAction.Created,
            Description = $"Booking created with {request.Passengers.Count} passenger(s) and {request.Segments.Count} segment(s)",
            PerformedByType = ActorType.Customer,
            PerformedAt = DateTime.UtcNow
        };
        await historyRepo.AddAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Booking created: {BookingReference}", booking.BookingReference);

        return Result<CreateBookingResult>.Success(new CreateBookingResult(
            booking.Id,
            booking.BookingReference,
            booking.TotalAmount,
            booking.Currency,
            booking.ExpiresAt
        ));
    }

    private async Task<Result<(List<BookingSegment> Segments, decimal BaseFare, decimal TaxAmount)>> 
        ValidateSegmentsAndCalculatePricing(List<BookingSegmentInput> segmentInputs, int passengerCount, CancellationToken cancellationToken)
    {
        var flightRepo = _unitOfWork.Repository<Flight>();
        var pricingRepo = _unitOfWork.Repository<FlightPricing>();
        var segments = new List<BookingSegment>();
        decimal totalBaseFare = 0;
        decimal totalTax = 0;

        foreach (var input in segmentInputs)
        {
            var flight = await flightRepo.GetByIdAsync(input.FlightId, cancellationToken);
            if (flight == null)
                return Result<(List<BookingSegment>, decimal, decimal)>.Failure($"Flight with ID '{input.FlightId}' not found");

            if (!flight.IsActive || flight.Status == FlightStatus.Cancelled)
                return Result<(List<BookingSegment>, decimal, decimal)>.Failure($"Flight '{flight.FlightNumber}' is not available for booking");

            if (flight.ScheduledDepartureTime <= DateTime.UtcNow)
                return Result<(List<BookingSegment>, decimal, decimal)>.Failure($"Flight '{flight.FlightNumber}' has already departed");

            // Get pricing for the cabin class
            var pricing = await pricingRepo.FirstOrDefaultAsync(
                p => p.FlightId == input.FlightId && p.CabinClass == input.CabinClass && p.IsActive,
                cancellationToken);

            if (pricing == null)
                return Result<(List<BookingSegment>, decimal, decimal)>.Failure(
                    $"No pricing found for flight '{flight.FlightNumber}' in {input.CabinClass} class");

            if (pricing.AvailableSeats < passengerCount)
                return Result<(List<BookingSegment>, decimal, decimal)>.Failure(
                    $"Not enough seats available in {input.CabinClass} class on flight '{flight.FlightNumber}'");

            decimal segmentBaseFare = pricing.CurrentPrice * passengerCount;
            decimal segmentTax = pricing.TaxAmount * passengerCount;

            var segment = new BookingSegment
            {
                Id = Guid.NewGuid(),
                FlightId = input.FlightId,
                SegmentOrder = input.SegmentOrder,
                CabinClass = input.CabinClass,
                BaseFarePerPax = pricing.CurrentPrice,
                TaxPerPax = pricing.TaxAmount,
                SegmentSubtotal = segmentBaseFare + segmentTax,
                Status = SegmentStatus.Confirmed,
                CheckInOpenAt = flight.ScheduledDepartureTime.AddHours(-24),
                CheckedBaggageAllowanceKg = GetBaggageAllowance(input.CabinClass),
                CabinBaggageAllowanceKg = 7
            };

            segments.Add(segment);
            totalBaseFare += segmentBaseFare;
            totalTax += segmentTax;
        }

        return Result<(List<BookingSegment>, decimal, decimal)>.Success((segments, totalBaseFare, totalTax));
    }
    private static int GetBaggageAllowance(FlightClass cabinClass)
    {
        return cabinClass switch
        {
            FlightClass.First => 40,
            FlightClass.Business => 32,
            FlightClass.PremiumEconomy => 25,
            FlightClass.Economy => 23,
            _ => 23
        };
    }

    private static Passenger CreatePassenger(Guid bookingId, PassengerInput input)
    {
        return new Passenger
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            PassengerType = input.PassengerType,
            Title = input.Title,
            FirstName = input.FirstName,
            MiddleName = input.MiddleName,
            LastName = input.LastName,
            DateOfBirth = input.DateOfBirth,
            Gender = input.Gender,
            Nationality = input.Nationality,
            PassportNumber = input.PassportNumber,
            PassportIssuingCountry = input.PassportIssuingCountry,
            PassportExpiryDate = input.PassportExpiryDate,
            Email = input.Email,
            Phone = input.Phone,
            MealPreference = input.MealPreference,
            SpecialAssistance = input.SpecialAssistance,
            FrequentFlyerNumber = input.FrequentFlyerNumber,
            FrequentFlyerAirlineId = input.FrequentFlyerAirlineId,
            IsPrimaryContact = input.IsPrimaryContact,
            IsLeadPassenger = input.IsLeadPassenger
        };
    }

    private static string GenerateBookingReference()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

