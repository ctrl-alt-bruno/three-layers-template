using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ThreeLayers.Business.Interfaces;
using ThreeLayers.Business.Models;
using ThreeLayers.Data.Context;

namespace ThreeLayers.Data.Repository;

public abstract class Repository<TEntity>(MyDbContext dbContext)
    : ICommandRepository<TEntity>, IQueryRepository<TEntity>
    where TEntity : Entity, new()
{
    protected readonly MyDbContext DbContext = dbContext;
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        DbSet.Remove(new TEntity() { Id = id });
        await SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await DbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        DbContext?.Dispose();
    }
}