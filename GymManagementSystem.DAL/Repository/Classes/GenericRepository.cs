using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace GymManagementSystem.DAL.Repository.Classes;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    private readonly GymDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(GymDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
    {
        IQueryable<TEntity> query = _dbSet;
        if (!tracking)query = query.AsNoTracking();
        return await query.ToListAsync(ct);
    }
    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, ct);
    public void Add(TEntity entity)
    {
        _dbSet.AddAsync(entity);
    }
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
    public void  Delete(TEntity entity)
    {
        _dbSet.Remove(entity);

    }
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default) => await _context.Set<TEntity>().AnyAsync(predicate, ct);
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)=> await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, ct);
    public Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate = null,CancellationToken ct = default)
    => predicate is null? _dbSet.CountAsync(ct): _dbSet.CountAsync(predicate, ct);
}
