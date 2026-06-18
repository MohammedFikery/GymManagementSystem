using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.UnitOFWork.Interfaces
{
    public interface IUnitOFWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        Task<int> SaveChangesAsync(CancellationToken ct=default);

        public ISessionRepository SessionRepository { get; }

    }
}
