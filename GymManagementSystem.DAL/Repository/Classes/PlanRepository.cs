
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Repositories
{
    public  class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _context;

        public PlanRepository(GymDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query = _context.Plans;

            if (!tracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(ct);
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)=>
        
            await _context.Plans.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        

        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            await _context.Plans.AddAsync(plan, ct);
            return await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            _context.Plans.Update(plan);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var plan = await _context.Plans.FindAsync(new object[] { id }, ct);
            if (plan != null)
            {
                _context.Plans.Remove(plan);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}