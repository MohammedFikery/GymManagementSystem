using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;


namespace GymManagementSystem.DAL.Repository.Classes
{
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

            if (!tracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(ct);
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, ct);
        public virtual async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);
            return await _context.SaveChangesAsync(ct);
        }
        public virtual async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync(ct);
        }
        public virtual async Task<int> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync(new object[] { id }, ct);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(ct);
            }
            return 0;
        }
    }
}
