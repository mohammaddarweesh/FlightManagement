using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Customers.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid CustomerId) : IQuery<CustomerDto>;

