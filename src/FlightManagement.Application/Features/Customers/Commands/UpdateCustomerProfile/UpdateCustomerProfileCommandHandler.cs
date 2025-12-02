using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Customers.Commands.UpdateCustomerProfile;

public class UpdateCustomerProfileCommandHandler : ICommandHandler<UpdateCustomerProfileCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCustomerProfileCommandHandler> _logger;

    public UpdateCustomerProfileCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateCustomerProfileCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating customer profile: {CustomerId}", request.CustomerId);

        var repository = _unitOfWork.Repository<Customer>();

        var customer = await repository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (customer == null)
        {
            _logger.LogWarning("Update profile failed: Customer not found {CustomerId}", request.CustomerId);
            return Result.Failure("Customer not found");
        }

        customer.FirstName = request.FirstName.Trim();
        customer.LastName = request.LastName.Trim();
        customer.PhoneNumber = request.PhoneNumber?.Trim();
        customer.DateOfBirth = request.DateOfBirth;
        customer.AddressLine1 = request.AddressLine1?.Trim();
        customer.AddressLine2 = request.AddressLine2?.Trim();
        customer.City = request.City?.Trim();
        customer.State = request.State?.Trim();
        customer.PostalCode = request.PostalCode?.Trim();
        customer.Country = request.Country?.Trim();
        customer.PreferredLanguage = request.PreferredLanguage?.Trim();
        customer.PreferredCurrency = request.PreferredCurrency?.Trim();
        customer.ReceivePromotionalEmails = request.ReceivePromotionalEmails;
        customer.ReceiveSmsNotifications = request.ReceiveSmsNotifications;
        customer.UpdatedAt = DateTime.UtcNow;

        repository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Customer profile updated: {CustomerId}", request.CustomerId);
        return Result.Success("Profile updated successfully");
    }
}

