using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanRepository _planRepository; 
        private readonly GymDbContext _db;                

        // Constructor Injection
        public PlanController(IPlanRepository planRepository, GymDbContext db)
        {
            _planRepository = planRepository;
            _db = db;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);
            if (plan == null)
                return RedirectToAction("Index");

            return View(plan);
        }
    }
}