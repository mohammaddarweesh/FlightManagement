using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;

