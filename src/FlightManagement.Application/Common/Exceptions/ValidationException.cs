using FluentValidation.Results;

namespace FlightManagement.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when validation fails.
/// Contains a dictionary of validation errors grouped by property name.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Dictionary of validation errors.
    /// Key: Property name
    /// Value: Array of error messages for that property
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.ToArray());
    }

    public ValidationException(string propertyName, string errorMessage)
        : this()
    {
        Errors = new Dictionary<string, string[]>
        {
            { propertyName, new[] { errorMessage } }
        };
    }
}

