using SendEmail.Business.Interfaces.Repositories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SendEmail.Business.Models;

namespace SendEmail.Data.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable where TEntity : Entity
{
    private bool _isDisposed;
    protected readonly SqlContext _db;
    protected readonly DbSet<TEntity> _dbSet;
    
    public BaseRepository(SqlContext db)
    {
        _db = db;
        _dbSet = db.Set<TEntity>();
    }
    
    #region Default
    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }
    
    public void Add(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Update(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }
    
    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
    
    public void Remove(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
    
    public async Task<ICollection<TEntity>> Search(Expression<Func<TEntity, bool>> predicate)
    {
        var query = _dbSet.AsQueryable<TEntity>();
        return await query.AsNoTracking().Where(predicate).ToListAsync();
    }
    
    public async Task<ICollection<TEntity>> Search(
        Expression<Func<TEntity, bool>> predicate,
        params Expression<Func<TEntity, Entity>>[] includes)
    {
        var query = _dbSet.AsQueryable<TEntity>();
        includes.ToList().ForEach(i => query = query.AsNoTracking().Include(i));
        return await query.AsNoTracking().Where(predicate).ToListAsync();
    }

    public async Task<TEntity> GetById(Guid id)
    {
        return (await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id))!;
    }
    
    public async Task<TEntity> GetById(Guid id, params Expression<Func<TEntity, Entity>>[] includes)
    {
        var query = _dbSet.AsQueryable<TEntity>();
        includes.ToList().ForEach(i => query = query.AsNoTracking().Include(i));
        return (await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id))!;
    }

    public async Task<ICollection<TEntity>> GetAll()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }
    
    public async Task<ICollection<TEntity>> GetAll(params Expression<Func<TEntity, Entity>>[] includes)
    {
        var query = _dbSet.AsQueryable<TEntity>();
        includes.ToList().ForEach(i => query = query.AsNoTracking().Include(i));
        return await query.AsNoTracking().ToListAsync();
    }
    
    public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return (await _dbSet.AsNoTracking()
            .Where(predicate)
            .FirstOrDefaultAsync())!;
    }
    
    public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, Entity>>[] includes)
    {
        var query = _dbSet.AsQueryable<TEntity>();
        includes.ToList().ForEach(i => query = query.AsNoTracking().Include(i));
        return (await query.AsNoTracking().Where(predicate).FirstOrDefaultAsync())!;
    }


    public async Task<bool> Exists(Guid id)
    {
        return await _dbSet.AsNoTracking().AnyAsync(x => x.Id == id);
    }
    
    public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().AnyAsync(predicate);
    }

    public void ClearTrackedEntity()
    {
        _db.ChangeTracker.Clear();
    }

    public async Task SaveChanges()
    {
        await _db.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        Dispose(_isDisposed);
    }
    #endregion

    #region Private Methods
    private void Dispose(bool disposing)
    {
        if (_isDisposed) return;
    
        if (disposing)
        {
            _db?.Dispose();
        }
        _isDisposed = true;
    }
    #endregion
}