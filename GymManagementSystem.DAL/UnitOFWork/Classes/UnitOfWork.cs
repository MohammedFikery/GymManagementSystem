using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Classes;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using GymManagementSystem.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.UnitOFWork.Classes
{
    public class UnitOfWork : IUnitOFWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity).Name;
            if (_repositories.TryGetValue(entityType,out object? value))
                return (IGenericRepository<TEntity>)value!;
            else
            {
                var repositoryInstance = new GenericRepository<TEntity>(_dbContext);
                _repositories[entityType]= repositoryInstance;
                return repositoryInstance;
            }

        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);
    }
}
