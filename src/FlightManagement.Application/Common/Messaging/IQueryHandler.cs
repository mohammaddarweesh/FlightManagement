using FlightManagement.Application.Common.Models;
using MediatR;

namespace FlightManagement.Application.Common.Messaging;

/// <summary>
/// Base interface for query handlers
/// </summary>
/// <typeparam name="TQuery">The type of query being handled</typeparam>
/// <typeparam name="TResponse">The type of the response</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}

