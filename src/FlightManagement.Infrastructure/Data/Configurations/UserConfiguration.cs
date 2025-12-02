using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        // Store UserType as string for readability
        builder.Property(u => u.UserType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(UserType.Customer);

        builder.Property(u => u.EmailVerificationToken)
            .HasMaxLength(128);

        builder.Property(u => u.PasswordResetToken)
            .HasMaxLength(128);

        builder.HasIndex(u => u.EmailVerificationToken)
            .HasFilter("\"EmailVerificationToken\" IS NOT NULL");

        builder.HasIndex(u => u.PasswordResetToken)
            .HasFilter("\"PasswordResetToken\" IS NOT NULL");

        // Soft delete filter
        builder.HasQueryFilter(u => !u.IsDeleted);

        // One-to-one relationship with Customer
        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

