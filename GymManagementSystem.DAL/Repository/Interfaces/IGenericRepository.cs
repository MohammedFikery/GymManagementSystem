using GymManagementSystem.DAL.Models;
using System.Linq.Expressions;

namespace GymManagementSystem.DAL.Repository.Interfaces;

/// <summary>
/// Generic repository interface for CRUD operations on entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
{
    Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);
     public void Add (TEntity entity);
    public void Update(TEntity entity);
    public void Delete(TEntity entity);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate=null, CancellationToken ct = default);

}
