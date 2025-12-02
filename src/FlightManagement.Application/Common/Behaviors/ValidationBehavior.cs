using FluentValidation;
using MediatR;

namespace FlightManagement.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that automatically validates incoming requests.
/// If validation fails, throws a ValidationException with all validation errors.
/// </summary>
/// <typeparam name="TRequest">The request type being validated</typeparam>
/// <typeparam name="TResponse">The response type</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // If no validators exist for this request, continue to handler
        if (!_validators.Any())
        {
            return await next();
        }

        // Create validation context
        var context = new ValidationContext<TRequest>(request);

        // Run all validators and collect results
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Get all failures
        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();

        // If there are failures, throw ValidationException
        if (failures.Count > 0)
        {
            throw new Exceptions.ValidationException(failures);
        }

        return await next();
    }
}

