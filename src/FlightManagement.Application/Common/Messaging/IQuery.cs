using FlightManagement.Application.Common.Models;
using MediatR;

namespace FlightManagement.Application.Common.Messaging;

/// <summary>
/// Base interface for queries
/// </summary>
/// <typeparam name="TResponse">The type of the response</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

