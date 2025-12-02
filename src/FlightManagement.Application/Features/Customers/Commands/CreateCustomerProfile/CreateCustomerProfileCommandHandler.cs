using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Customers.Commands.CreateCustomerProfile;

public class CreateCustomerProfileCommandHandler : ICommandHandler<CreateCustomerProfileCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<CreateCustomerProfileCommandHandler> _logger;

    public CreateCustomerProfileCommandHandler(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<CreateCustomerProfileCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateCustomerProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating customer profile for user: {UserId}", request.UserId);

        var userRepository = _unitOfWork.Repository<User>();
        var customerRepository = _unitOfWork.Repository<Customer>();

        // Verify user exists
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Create profile failed: User not found {UserId}", request.UserId);
            return Result<Guid>.Failure("User not found");
        }

        // Check if customer profile already exists
        var existingCustomer = await customerRepository.FirstOrDefaultAsync(
            c => c.UserId == request.UserId, cancellationToken);

        if (existingCustomer != null)
        {
            _logger.LogWarning("Create profile failed: Profile already exists for user {UserId}", request.UserId);
            return Result<Guid>.Failure("Customer profile already exists for this user");
        }

        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            DateOfBirth = request.DateOfBirth,
            AddressLine1 = request.AddressLine1?.Trim(),
            AddressLine2 = request.AddressLine2?.Trim(),
            City = request.City?.Trim(),
            State = request.State?.Trim(),
            PostalCode = request.PostalCode?.Trim(),
            Country = request.Country?.Trim(),
            PreferredLanguage = request.PreferredLanguage?.Trim(),
            PreferredCurrency = request.PreferredCurrency?.Trim(),
            ReceivePromotionalEmails = request.ReceivePromotionalEmails,
            ReceiveSmsNotifications = request.ReceiveSmsNotifications,
            CreatedAt = DateTime.UtcNow
        };

        await customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Send welcome email
        await _emailService.SendWelcomeEmailAsync(user.Email, customer.FullName, cancellationToken);

        _logger.LogInformation("Customer profile created: {Id} for user: {UserId}", customer.Id, request.UserId);
        return Result<Guid>.Success(customer.Id);
    }
}

