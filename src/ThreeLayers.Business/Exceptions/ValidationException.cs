namespace ThreeLayers.Business.Exceptions;

/// <summary>
/// Exception thrown when semantic validation fails.
/// Results in HTTP 422 Unprocessable Entity.
/// </summary>
public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(string message) : base(message)
    {
        Errors = new List<string> { message };
    }

    public ValidationException(IEnumerable<string> errors) 
        : base("One or more validation errors occurred.")
    {
        Errors = errors.ToList();
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
        Errors = new List<string> { message };
    }
}