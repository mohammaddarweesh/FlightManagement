using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

public class User : BaseEntity, ISoftDeletable
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // User type for authorization
    public UserType UserType { get; set; } = UserType.Customer;

    // Email verification
    public bool IsEmailVerified { get; set; }
    public string? EmailVerificationToken { get; set; }
    public DateTime? EmailVerificationTokenExpiry { get; set; }

    // Password reset
    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }

    // Account status
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }

    // Soft delete
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    // Navigation
    public Customer? Customer { get; set; }
}

