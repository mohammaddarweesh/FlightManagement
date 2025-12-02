using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Entities;

public class Customer : BaseEntity, ISoftDeletable
{
    // Link to user
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    // Personal information
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    // Address
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    
    // Preferences
    public string? PreferredLanguage { get; set; }
    public string? PreferredCurrency { get; set; }
    public bool ReceivePromotionalEmails { get; set; }
    public bool ReceiveSmsNotifications { get; set; }
    
    // Soft delete
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<PromotionUsage> PromotionUsages { get; set; } = new List<PromotionUsage>();

    // Computed property
    public string FullName => $"{FirstName} {LastName}".Trim();
}

