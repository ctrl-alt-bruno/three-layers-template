namespace ThreeLayers.Business.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated.
/// Results in HTTP 409 Conflict.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }

    public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
    {
    }
}