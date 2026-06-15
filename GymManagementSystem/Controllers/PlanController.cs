using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystem.Controllers
{
    public class PlanController : Controller
    {

        private readonly IPlanServices _planServices;

        public PlanController(IPlanServices planServices)
        {
            _planServices = planServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        => View(await _planServices.GetAllPlansAsync(ct: ct));
        

        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var plan = await _planServices.GetPlanByIdAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction("Index");
            }

            return View(plan);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,UpdatePlaneViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _planServices.upatePlanAsync(id, model, ct);
                if (result)
                {
                    TempData["SuccessMessage"] = "Member created successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to create member. Please try again.";
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

    }
}