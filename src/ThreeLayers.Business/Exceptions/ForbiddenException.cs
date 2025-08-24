namespace ThreeLayers.Business.Exceptions;

/// <summary>
/// Exception thrown when user is authenticated but not authorized to access the resource.
/// Results in HTTP 403 Forbidden.
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message = "You do not have permission to access this resource.") : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}