using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GymManagementSystem.DAL.Repository.Interfaces
{  
    /// <summary>
   /// Generic repository interface for CRUD operations on entities.
   /// </summary>
   /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default);

        Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<int> AddAsync(TEntity entity, CancellationToken ct = default);

        Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default);

        Task<int> DeleteAsync(int id, CancellationToken ct = default);
         
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);

    }
}
