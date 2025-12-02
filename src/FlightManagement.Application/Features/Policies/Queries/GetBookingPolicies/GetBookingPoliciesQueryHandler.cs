using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Policies.Queries.GetBookingPolicies;

public class GetBookingPoliciesQueryHandler : IQueryHandler<GetBookingPoliciesQuery, BookingPolicyListResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingPoliciesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BookingPolicyListResult>> Handle(
        GetBookingPoliciesQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _unitOfWork.Repository<BookingPolicy>().Query();

        if (request.Type.HasValue)
            baseQuery = baseQuery.Where(p => p.Type == request.Type.Value);

        if (request.IsActive.HasValue)
            baseQuery = baseQuery.Where(p => p.IsActive == request.IsActive.Value);

        if (request.AirlineId.HasValue)
            baseQuery = baseQuery.Where(p => p.AirlineId == request.AirlineId.Value);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var query = baseQuery
            .Include(p => p.Airline)
            .Include(p => p.DepartureAirport)
            .Include(p => p.ArrivalAirport);

        var policies = await query
            .OrderByDescending(p => p.Priority)
            .ThenBy(p => p.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = policies.Select(p => new BookingPolicyDto(
            p.Id,
            p.Code,
            p.Name,
            p.Description,
            p.Type,
            p.Value,
            p.ErrorMessage,
            p.AirlineId,
            p.Airline?.Name,
            p.DepartureAirportId,
            p.DepartureAirport?.IataCode,
            p.ArrivalAirportId,
            p.ArrivalAirport?.IataCode,
            p.CabinClass,
            p.Priority,
            p.IsActive
        )).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return Result<BookingPolicyListResult>.Success(new BookingPolicyListResult(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        ));
    }
}

