using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Services;

public class AvailabilityService : IAvailabilityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AvailabilityService> _logger;

    public AvailabilityService(IUnitOfWork unitOfWork, ILogger<AvailabilityService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AvailabilityCheckResult> CheckAvailabilityAsync(
        Guid flightId,
        FlightClass cabinClass,
        int requestedSeats,
        CancellationToken cancellationToken = default)
    {
        var flight = await _unitOfWork.Repository<Flight>()
            .Query()
            .Include(f => f.FlightPricings)
            .Include(f => f.Airline)
            .FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);

        if (flight == null)
            return new AvailabilityCheckResult(false, 0, 0, 0, "Flight not found");

        var pricing = flight.FlightPricings.FirstOrDefault(p => p.CabinClass == cabinClass);
        if (pricing == null)
            return new AvailabilityCheckResult(false, 0, 0, 0, $"No {cabinClass} class available on this flight");

        // Get overbooking allowance
        var overbookingAllowance = await GetOverbookingAllowanceAsync(
            flight.AirlineId, flight.DepartureAirportId, flight.ArrivalAirportId, cabinClass, pricing.TotalSeats, cancellationToken);

        int maxBookableSeats = pricing.TotalSeats + overbookingAllowance;
        int currentBookings = pricing.TotalSeats - pricing.AvailableSeats;
        int availableForBooking = maxBookableSeats - currentBookings;

        bool isAvailable = availableForBooking >= requestedSeats;
        string? message = isAvailable ? null : $"Only {availableForBooking} seats available (including overbooking allowance)";

        return new AvailabilityCheckResult(
            isAvailable,
            pricing.AvailableSeats,
            maxBookableSeats,
            overbookingAllowance,
            message
        );
    }

    public async Task<int> GetMaxBookableSeatsAsync(
        Guid flightId,
        FlightClass cabinClass,
        CancellationToken cancellationToken = default)
    {
        var flight = await _unitOfWork.Repository<Flight>()
            .Query()
            .Include(f => f.FlightPricings)
            .FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);

        if (flight == null) return 0;

        var pricing = flight.FlightPricings.FirstOrDefault(p => p.CabinClass == cabinClass);
        if (pricing == null) return 0;

        var overbookingAllowance = await GetOverbookingAllowanceAsync(
            flight.AirlineId, flight.DepartureAirportId, flight.ArrivalAirportId, cabinClass, pricing.TotalSeats, cancellationToken);

        return pricing.TotalSeats + overbookingAllowance;
    }

    public async Task<PolicyValidationResult> ValidateBookingPoliciesAsync(
        Guid flightId,
        FlightClass cabinClass,
        DateTime bookingDate,
        int passengerCount,
        CancellationToken cancellationToken = default)
    {
        var violations = new List<PolicyViolation>();

        var flight = await _unitOfWork.Repository<Flight>()
            .Query()
            .FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);

        if (flight == null)
        {
            violations.Add(new PolicyViolation("FLIGHT_NOT_FOUND", "Flight Not Found", "The specified flight does not exist"));
            return new PolicyValidationResult(false, violations);
        }

        // Get applicable policies
        var policyRepo = _unitOfWork.Repository<BookingPolicy>();
        var policies = await policyRepo.Query()
            .Where(p => p.IsActive)
            .Where(p => p.AirlineId == null || p.AirlineId == flight.AirlineId)
            .Where(p => p.DepartureAirportId == null || p.DepartureAirportId == flight.DepartureAirportId)
            .Where(p => p.ArrivalAirportId == null || p.ArrivalAirportId == flight.ArrivalAirportId)
            .Where(p => p.CabinClass == null || p.CabinClass == cabinClass)
            .OrderByDescending(p => p.Priority)
            .ToListAsync(cancellationToken);

        var hoursBeforeDeparture = (flight.ScheduledDepartureTime - bookingDate).TotalHours;

        foreach (var policy in policies)
        {
            bool violated = policy.Type switch
            {
                PolicyType.MinimumAdvancePurchase => hoursBeforeDeparture < policy.Value,
                PolicyType.MaximumAdvanceBooking => hoursBeforeDeparture > policy.Value * 24, // Value in days
                PolicyType.MinimumPassengers => passengerCount < policy.Value,
                PolicyType.MaximumPassengers => passengerCount > policy.Value,
                _ => false
            };

            if (violated)
            {
                violations.Add(new PolicyViolation(policy.Code, policy.Name, policy.ErrorMessage));
            }
        }

        return new PolicyValidationResult(violations.Count == 0, violations);
    }

    public async Task<BlackoutCheckResult> CheckBlackoutDatesAsync(
        Guid flightId,
        DateTime travelDate,
        bool checkPromotions = false,
        CancellationToken cancellationToken = default)
    {
        var flight = await _unitOfWork.Repository<Flight>()
            .Query()
            .FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);

        if (flight == null)
            return new BlackoutCheckResult(false, false, false, null, null);

        var blackoutRepo = _unitOfWork.Repository<BlackoutDate>();
        var blackout = await blackoutRepo.Query()
            .Where(b => b.IsActive)
            .Where(b => b.StartDate <= travelDate && b.EndDate >= travelDate)
            .Where(b => b.AirlineId == null || b.AirlineId == flight.AirlineId)
            .Where(b => b.DepartureAirportId == null || b.DepartureAirportId == flight.DepartureAirportId)
            .Where(b => b.ArrivalAirportId == null || b.ArrivalAirportId == flight.ArrivalAirportId)
            .FirstOrDefaultAsync(cancellationToken);

        if (blackout == null)
            return new BlackoutCheckResult(false, false, false, null, null);

        return new BlackoutCheckResult(
            true,
            blackout.BlocksBookings,
            blackout.BlocksPromotions,
            blackout.Name,
            blackout.Description
        );
    }

    private async Task<int> GetOverbookingAllowanceAsync(
        Guid airlineId,
        Guid departureAirportId,
        Guid arrivalAirportId,
        FlightClass cabinClass,
        int totalSeats,
        CancellationToken cancellationToken)
    {
        var policyRepo = _unitOfWork.Repository<OverbookingPolicy>();
        var policy = await policyRepo.Query()
            .Where(p => p.IsActive && p.AirlineId == airlineId)
            .Where(p => p.DepartureAirportId == null || p.DepartureAirportId == departureAirportId)
            .Where(p => p.ArrivalAirportId == null || p.ArrivalAirportId == arrivalAirportId)
            .Where(p => p.CabinClass == null || p.CabinClass == cabinClass)
            .OrderByDescending(p => p.Priority)
            .FirstOrDefaultAsync(cancellationToken);

        if (policy == null) return 0;

        // Calculate overbooking allowance
        int percentageAllowance = (int)Math.Ceiling(totalSeats * (policy.MaxOverbookingPercentage / 100));
        int maxAllowance = policy.MaxOverbookedSeats ?? int.MaxValue;

        return Math.Min(percentageAllowance, maxAllowance);
    }
}

