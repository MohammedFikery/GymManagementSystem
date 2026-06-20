using GymManagementSystem.DAL.Models;


namespace GymManagementSystem.DAL.Repository.Interfaces;

public interface ISessionRepository:IGenericRepository<Session>
{
    Task<IEnumerable<Session>?> GetAllSessionswithTrainerAndCategery(CancellationToken ct=default);
    Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default);
    Task<Dictionary<int, int>> GetBookedSlotsCountAsync( IEnumerable<int> sessionIds,CancellationToken ct = default);
    Task<Session> GetSessionByIdwithTrainerAndCategery(int sessionId, CancellationToken ct = default);
}
