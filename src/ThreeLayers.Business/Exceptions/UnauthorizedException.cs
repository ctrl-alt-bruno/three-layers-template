namespace ThreeLayers.Business.Exceptions;

/// <summary>
/// Exception thrown when authentication is required but not provided or invalid.
/// Results in HTTP 401 Unauthorized.
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Authentication is required to access this resource.") : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}