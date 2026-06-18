using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repository.Interfaces
{
    public interface ISessionRepository:IGenericRepository<Session>
    {
        Task<IEnumerable<Session>?> GetAllSessionswithTrainerAndCategery(CancellationToken ct=default);
        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);
        Task<Dictionary<int, int>> GetBookedSlotsCountAsync( IEnumerable<int> sessionIds,CancellationToken ct = default);
    }
}
