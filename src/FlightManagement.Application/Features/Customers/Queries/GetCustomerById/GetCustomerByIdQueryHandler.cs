using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Customers.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCustomerByIdQueryHandler> _logger;

    public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCustomerByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting customer by id: {CustomerId}", request.CustomerId);

        var repository = _unitOfWork.Repository<Customer>();

        var customer = await repository.GetByIdAsync(request.CustomerId, cancellationToken);

        if (customer == null)
        {
            _logger.LogWarning("Customer not found: {CustomerId}", request.CustomerId);
            return Result<CustomerDto>.Failure("Customer not found");
        }

        var dto = new CustomerDto(
            customer.Id,
            customer.UserId,
            customer.FirstName,
            customer.LastName,
            customer.FullName,
            customer.PhoneNumber,
            customer.DateOfBirth,
            new AddressDto(
                customer.AddressLine1,
                customer.AddressLine2,
                customer.City,
                customer.State,
                customer.PostalCode,
                customer.Country
            ),
            new PreferencesDto(
                customer.PreferredLanguage,
                customer.PreferredCurrency,
                customer.ReceivePromotionalEmails,
                customer.ReceiveSmsNotifications
            ),
            customer.CreatedAt,
            customer.UpdatedAt
        );

        return Result<CustomerDto>.Success(dto);
    }
}

