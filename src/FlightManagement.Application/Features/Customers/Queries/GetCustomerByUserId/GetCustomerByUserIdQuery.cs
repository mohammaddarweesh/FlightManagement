using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Customers.Queries.GetCustomerById;

namespace FlightManagement.Application.Features.Customers.Queries.GetCustomerByUserId;

public record GetCustomerByUserIdQuery(Guid UserId) : IQuery<CustomerDto>;

