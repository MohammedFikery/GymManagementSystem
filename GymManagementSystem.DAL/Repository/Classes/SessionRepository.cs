using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repository.Classes;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    private readonly GymDbContext _context;

    public SessionRepository(GymDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Session>?> GetAllSessionswithTrainerAndCategery(CancellationToken ct)
    {
        var sessions = _context.Sessions.AsNoTracking().Include(s=>s.Trainer).Include(s=>s.Category);
        return await sessions.ToListAsync();
    }

    public async Task<Dictionary<int, int>>GetBookedSlotsCountAsync(IEnumerable<int> sessionIds,CancellationToken ct = default)
    {
        return await _context.Bookings.Where(b => sessionIds.Contains(b.SessionId))

            .GroupBy(b => b.SessionId)
            .Select(g => new{ SessionId = g.Key, Count = g.Count()})
            .ToDictionaryAsync(x => x.SessionId,x => x.Count, ct);
    }

    public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct = default)
    => await _context.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId, ct);
    public async Task<Session?> GetSessionByIdwithTrainerAndCategery(int sessionId, CancellationToken ct = default)
    {
        return await _context.Sessions.AsNoTracking().Include(x => x.Trainer).Include(c => c.Category).FirstOrDefaultAsync(x=>x.Id==sessionId);
    }
}
