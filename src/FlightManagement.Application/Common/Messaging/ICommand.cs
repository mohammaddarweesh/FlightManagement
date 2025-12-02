using FlightManagement.Application.Common.Models;
using MediatR;

namespace FlightManagement.Application.Common.Messaging;

/// <summary>
/// Marker interface for commands that don't return a value
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Base interface for commands that return a value
/// </summary>
/// <typeparam name="TResponse">The type of the response</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}

