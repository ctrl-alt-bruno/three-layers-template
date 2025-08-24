namespace ThreeLayers.Business.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
/// Results in HTTP 404 Not Found.
/// </summary>
public class EntityNotFoundException : Exception
{
    public string EntityName { get; }
    public object EntityId { get; }

    public EntityNotFoundException(string entityName, object entityId) 
        : base($"{entityName} with id '{entityId}' was not found.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    public EntityNotFoundException(string entityName, object entityId, Exception innerException) 
        : base($"{entityName} with id '{entityId}' was not found.", innerException)
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}