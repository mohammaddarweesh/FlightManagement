using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Customers.Queries.GetCustomerById;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Customers.Queries.GetCustomerByUserId;

public class GetCustomerByUserIdQueryHandler : IQueryHandler<GetCustomerByUserIdQuery, CustomerDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCustomerByUserIdQueryHandler> _logger;

    public GetCustomerByUserIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCustomerByUserIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting customer by user id: {UserId}", request.UserId);

        var repository = _unitOfWork.Repository<Customer>();

        var customer = await repository.FirstOrDefaultAsync(
            c => c.UserId == request.UserId, cancellationToken);

        if (customer == null)
        {
            _logger.LogWarning("Customer not found for user: {UserId}", request.UserId);
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

