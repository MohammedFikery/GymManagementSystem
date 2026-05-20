using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlanController:Controller
    {
        private readonly GymDbContext _db;

        public PlanController()
        {
            _db = new GymDbContext();

        }

        public async Task<IActionResult> Index()
        {
            var plans = await _db.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id) {
            var plan = await _db.Plans.FindAsync(id);
            if (plan == null)
            {
                return RedirectToAction("Index");
            }
            return View(plan);
        }
    }
}
