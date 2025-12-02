using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Policies.Queries.GetDynamicPricingRules;

public class GetDynamicPricingRulesQueryHandler : IQueryHandler<GetDynamicPricingRulesQuery, DynamicPricingRuleListResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDynamicPricingRulesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DynamicPricingRuleListResult>> Handle(
        GetDynamicPricingRulesQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _unitOfWork.Repository<DynamicPricingRule>().Query();

        if (request.RuleType.HasValue)
            baseQuery = baseQuery.Where(r => r.RuleType == request.RuleType.Value);

        if (request.IsActive.HasValue)
            baseQuery = baseQuery.Where(r => r.IsActive == request.IsActive.Value);

        if (request.AirlineId.HasValue)
            baseQuery = baseQuery.Where(r => r.AirlineId == request.AirlineId.Value);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var rules = await baseQuery
            .Include(r => r.Airline)
            .OrderByDescending(r => r.Priority)
            .ThenBy(r => r.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = rules.Select(r => new DynamicPricingRuleDto(
            r.Id,
            r.Name,
            r.Description,
            r.RuleType,
            r.AdjustmentPercentage,
            r.FixedAdjustment,
            r.Currency,
            r.Priority,
            r.ApplicableDays,
            r.SeasonType,
            r.SeasonStartDate,
            r.SeasonEndDate,
            r.MinBookingPercentage,
            r.MaxBookingPercentage,
            r.MinDaysBeforeDeparture,
            r.MaxDaysBeforeDeparture,
            r.StartHour,
            r.EndHour,
            r.AirlineId,
            r.Airline?.Name,
            r.CabinClass,
            r.IsActive
        )).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return Result<DynamicPricingRuleListResult>.Success(new DynamicPricingRuleListResult(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        ));
    }
}

