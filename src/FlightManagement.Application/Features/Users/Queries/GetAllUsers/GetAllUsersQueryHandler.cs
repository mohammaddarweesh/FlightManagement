using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, PagedUserResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PagedUserResult>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<User>().Query();

        if (request.UserType.HasValue)
            query = query.Where(u => u.UserType == request.UserType.Value);

        if (request.IsEmailVerified.HasValue)
            query = query.Where(u => u.IsEmailVerified == request.IsEmailVerified.Value);

        if (request.IsActive.HasValue)
            query = query.Where(u => u.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(u => u.Email.Contains(request.SearchTerm));

        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new UserDto(
                u.Id,
                u.Email,
                u.UserType,
                u.IsEmailVerified,
                u.IsActive,
                u.CreatedAt,
                u.LastLoginAt
            ))
            .ToListAsync(cancellationToken);

        return Result<PagedUserResult>.Success(new PagedUserResult(
            users,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        ));
    }
}

