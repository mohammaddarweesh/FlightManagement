using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user by id: {UserId}", request.UserId);

        var repository = _unitOfWork.Repository<User>();

        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", request.UserId);
            return Result<UserDto>.Failure("User not found");
        }

        var dto = new UserDto(
            user.Id,
            user.Email,
            user.IsEmailVerified,
            user.IsActive,
            user.LastLoginAt,
            user.CreatedAt
        );

        return Result<UserDto>.Success(dto);
    }
}

