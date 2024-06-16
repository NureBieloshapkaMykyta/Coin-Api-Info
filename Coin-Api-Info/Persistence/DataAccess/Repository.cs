using Application.Abstractions;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.DataAccess;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;

    private readonly DbSet<TEntity> _dbSet;

    public Repository(ApplicationDbContext chattoDbContext)
    {
        _context = chattoDbContext;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<Result<bool>> AddItemAsync(TEntity entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Result.Success(true);
        }
        catch
        {
            return Result.Failure<bool>($"Cannot add {typeof(TEntity).Name}");
        }
    }

    public async Task<Result<IEnumerable<TEntity>>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null)
    {
        try
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return Result.Success<IEnumerable<TEntity>>(await query.ToListAsync());
        }
        catch
        {
            return Result.Failure<IEnumerable<TEntity>>($"Cannot get {typeof(TEntity).Name}s");
        }

    }

    public async Task<Result<bool>> UpdateItemAsync(TEntity entityToUpdate)
    {
        try
        {
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToUpdate);
            }
            _dbSet.Update(entityToUpdate);

            await _context.SaveChangesAsync();

            return Result.Success<bool>();
        }
        catch
        {
            return Result.Failure<bool>($"Cannot update {typeof(TEntity).Name}");
        }
    }

    public async Task<Result<bool>> DeleteItemAsync(Guid id)
    {
        try
        {
            var entityToDelete = await _dbSet.FindAsync(id);

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);

            await _context.SaveChangesAsync();

            return Result.Success<bool>();
        }
        catch
        {
            return Result.Failure<bool>($"Cannot delete the {typeof(TEntity).Name}");
        }
    }
}
