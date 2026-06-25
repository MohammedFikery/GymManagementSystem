using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Trainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

[Authorize]

public class TrainerController : Controller
{
    private readonly ITrainerServices _trainerService;

    public TrainerController(ITrainerServices trainerService)
    {
        _trainerService = trainerService;
    }
    public async Task<IActionResult> Index(CancellationToken ct)
      => View(await _trainerService.GetAllTrainerAsync(ct));

    [HttpGet]
    public IActionResult Create()=> View(new CreateTrainerViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _trainerService.CreateTrainerAsync(model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create Trainer. Please try again.";
            }
            return RedirectToAction(nameof(Index));

        }
        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var result = await _trainerService.GetTrainerDetailesByIdAsync(id, ct);
        if (result is null)
        {
            TempData["ErrorMessage"] = "Trainer not found...!!!";
            return RedirectToAction(nameof(Index));
        }
        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct)
    {
        var trainer = await _trainerService.GetTrainerToUpdateAsync(id, ct);
        if (trainer is null)
        {
            TempData["ErrorMessage"] = "Trainer not found...!!!";
            return RedirectToAction(nameof(Index));
        }
        return View(trainer);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, TrainerToUpdateViewModel model, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _trainerService.UpdateTrainerAsync(id, model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create Trainer. Please try again.";
            }
            return RedirectToAction(nameof(Index));

        }


        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var trainer = await _trainerService.GetTrainerDetailesByIdAsync(id, ct);
        if (trainer == null)
        {
            TempData["Error Message"] = "Trainer not found...!!!";
            return RedirectToAction(nameof(Index));
        }

        return View();

    }
    [HttpPost]
    public IActionResult DeleteConfirmed(int id, CancellationToken ct)
    {
        var result = _trainerService.RemoveTrainerAsync(id, ct);
        if (result.Result)
        {
            TempData["SuccessMessage"] = "Trainer deleted successfully!";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to delete Trainer. Please try again.";
        }
        return RedirectToAction(nameof(Index));
    }
}
