using System.Linq.Expressions;
using ThreeLayers.Business.Models;

namespace ThreeLayers.Business.Interfaces;

public interface IQueryRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
}