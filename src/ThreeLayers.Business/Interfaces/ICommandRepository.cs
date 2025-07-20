using ThreeLayers.Business.Models;

namespace ThreeLayers.Business.Interfaces;

public interface ICommandRepository<in TEntity> : IDisposable where TEntity : Entity
{
	Task AddAsync(TEntity entity);
	Task UpdateAsync(TEntity entity);
	Task DeleteAsync(Guid id);
	Task<int> SaveChangesAsync();
}