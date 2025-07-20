namespace ThreeLayers.Business.Models;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}