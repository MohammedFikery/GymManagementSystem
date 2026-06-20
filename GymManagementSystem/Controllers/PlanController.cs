using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;


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
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var plan = await _planServices.GetPlanByIdAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }

            var model = new UpdatePlaneViewModel
            {
                PlanName = plan.Name,
                Description = plan.Description??"",
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };

            return View(model);
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
        public async Task<IActionResult> Activate (int planId,CancellationToken ct)
        {
            var result = await _planServices.ToggleActivetedAsync(planId, ct);
            if (result)
                TempData["SuccessMessage"] = "Plan status changed";
            else
                TempData["ErrorMessage"] = "Failed to Toggle Plan Status";
            return RedirectToAction(nameof(Index));

        }
    }
}