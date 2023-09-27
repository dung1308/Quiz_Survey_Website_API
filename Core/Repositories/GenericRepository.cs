
using Microsoft.EntityFrameworkCore;
using survey_quiz_app.Data;

namespace survey_quiz_app.Core.Repositories;

public class GenericRepository<T,TID> : IGenericRepository<T,TID> where T : class
{
    protected ApiDbContext _context;
    internal DbSet<T> _dbSet;
    protected readonly ILogger _logger;

    public GenericRepository( ApiDbContext context, ILogger logger)
    {
        _context = context;
        _logger = logger;
        this._dbSet = context.Set<T>();
    }
    public virtual async Task<bool> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return true;
    }

    // public virtual async Task<bool> getDateNow(T entity)
    // {
    //     await _dbSet.Set<DateTime>().FromSqlRaw("SELECT getDate()").FirstOrDefaultAsync();
    //     return true;
    // }

    public virtual async Task<bool> AddRange(List<T> entity)
    {
        
        await _dbSet.AddRangeAsync(entity);
        return true;
    }

    public virtual async Task<IEnumerable<T>> All()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<bool> Delete(T entity)
    {
        _dbSet.Remove(entity);
        return true;
    }

    public virtual async Task<bool> DeleteMulti(List<T> entity)
    {
        _dbSet.RemoveRange(entity);
        return true;
    }

    public virtual async Task<T?> GetById(TID id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<bool> Update(T entity)
    {
        _dbSet.Update(entity);
        return true;
    }
}