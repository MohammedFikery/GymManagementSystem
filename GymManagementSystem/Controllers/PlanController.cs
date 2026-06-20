using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers;

public class PlanController : Controller
{
    private readonly IPlanServices _planServices;

    public PlanController(IPlanServices planServices)=> _planServices = planServices;

    public async Task<IActionResult> Index(CancellationToken ct = default)=> View(await _planServices.GetAllPlansAsync(ct: ct));   
    public async Task<IActionResult> Details(int id, CancellationToken ct = default)
    {
        var plan = await _planServices.GetPlanByIdAsync(id, ct);
        if (plan is null)
        {
            TempData["ErrorMessage"] = "Plan not found.";
            return RedirectToAction(nameof(Index));
        }
        return View(plan);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
    {
        var model = await _planServices.GetPlanToUpdateAsync(id, ct);
        if (model is null)
        {
            TempData["ErrorMessage"] = "Plan not found or cannot be edited (has active memberships).";
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdatePlanViewModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _planServices.UpdatePlanAsync(id, model, ct);
        TempData[result ? "SuccessMessage" : "ErrorMessage"] =result ? "Plan updated successfully!" : "Failed to update plan. It may have active memberships.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, CancellationToken ct = default)
    {
        var result = await _planServices.ToggleActivationAsync(id, ct);
        TempData[result ? "SuccessMessage" : "ErrorMessage"] = result ? "Plan status changed." : "Failed to toggle plan status.";

        return RedirectToAction(nameof(Index));
    }
}
