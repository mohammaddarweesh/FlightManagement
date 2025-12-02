using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Common;

/// <summary>
/// Data transfer object for booking details.
/// </summary>
public record BookingDto(
    Guid Id,
    string BookingReference,
    Guid CustomerId,
    string CustomerName,
    BookingStatus Status,
    TripType TripType,
    DateTime BookingDate,
    DateTime? ExpiresAt,
    // Pricing
    decimal BaseFare,
    decimal TaxAmount,
    decimal ServiceFee,
    decimal SeatSelectionFees,
    decimal ExtrasFees,
    decimal DiscountAmount,
    decimal TotalAmount,
    string Currency,
    // Contact
    string ContactEmail,
    string ContactPhone,
    string? SpecialRequests,
    // Payment
    PaymentStatus PaymentStatus,
    decimal PaidAmount,
    DateTime? PaymentDueDate,
    // Cancellation
    Guid? CancellationPolicyId,
    string? CancellationPolicyName,
    DateTime? CancelledAt,
    string? CancellationReason,
    decimal? RefundAmount,
    RefundStatus? RefundStatus,
    // Related data
    List<PassengerDto> Passengers,
    List<BookingSegmentDto> Segments,
    List<BookingExtraDto> Extras,
    DateTime CreatedAt
);

/// <summary>
/// Simplified booking DTO for list views.
/// </summary>
public record BookingSimpleDto(
    Guid Id,
    string BookingReference,
    string CustomerName,
    BookingStatus Status,
    TripType TripType,
    string Route,
    DateTime DepartureDate,
    decimal TotalAmount,
    string Currency,
    PaymentStatus PaymentStatus,
    int PassengerCount,
    DateTime BookingDate
);

/// <summary>
/// Data transfer object for passenger details.
/// </summary>
public record PassengerDto(
    Guid Id,
    PassengerType PassengerType,
    string Title,
    string FirstName,
    string? MiddleName,
    string LastName,
    DateTime DateOfBirth,
    Gender Gender,
    string Nationality,
    string? PassportNumber,
    string? PassportIssuingCountry,
    DateTime? PassportExpiryDate,
    string? Email,
    string? Phone,
    MealPreference? MealPreference,
    string? SpecialAssistance,
    bool IsPrimaryContact,
    bool IsLeadPassenger,
    List<PassengerSeatDto> Seats
);

/// <summary>
/// Data transfer object for booking segment details.
/// </summary>
public record BookingSegmentDto(
    Guid Id,
    Guid FlightId,
    string FlightNumber,
    int SegmentOrder,
    FlightClass CabinClass,
    // Flight details
    string DepartureAirportCode,
    string DepartureAirportName,
    string ArrivalAirportCode,
    string ArrivalAirportName,
    DateTime ScheduledDepartureTime,
    DateTime ScheduledArrivalTime,
    string? DepartureTerminal,
    string? DepartureGate,
    // Pricing
    decimal BaseFarePerPax,
    decimal TaxPerPax,
    decimal SegmentSubtotal,
    // Status
    SegmentStatus Status,
    DateTime? CheckInOpenAt,
    // Baggage
    int CheckedBaggageAllowanceKg,
    int CabinBaggageAllowanceKg
);

/// <summary>
/// Data transfer object for passenger seat assignment.
/// </summary>
public record PassengerSeatDto(
    Guid Id,
    Guid BookingSegmentId,
    string FlightNumber,
    string SeatNumber,
    decimal SeatFee,
    SeatAssignmentType AssignmentType
);

/// <summary>
/// Data transfer object for booking extra details.
/// </summary>
public record BookingExtraDto(
    Guid Id,
    ExtraType ExtraType,
    string Description,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice,
    string Currency,
    ExtraStatus Status,
    Guid? BookingSegmentId,
    Guid? PassengerId,
    string? PassengerName
);

