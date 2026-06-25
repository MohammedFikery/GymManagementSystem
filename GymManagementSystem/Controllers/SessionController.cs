using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers;

[Authorize]

public class SessionController : Controller
{
    private readonly ISessionServices _sessionServices;
    public SessionController( ISessionServices sessionServices)
    {
        _sessionServices = sessionServices;
    }
    public async Task<IActionResult> Index()
    {
        var sessions = await _sessionServices.GetAllSessionAsync();
        return View(sessions);
    }
    [HttpGet]
     public async Task<IActionResult> Create()
    {
        await PopulateDropDownsAsync();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateSessionViewModel model,CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropDownsAsync();

            return View(model);
        }
        var result = await _sessionServices.createSessionAsync(model,ct);
        if (result.success)
        {
          TempData["SuccessMessage"] = "Session created successfully.";
           return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = result.error;
        await PopulateDropDownsAsync();
        return View(model);
    }
    private Task PopulateDropDownsAsync()
    {
        ViewBag.Trainers = new SelectList(_sessionServices.GetTrainersForDropDownAsync().Result, "id", "Name");
        ViewBag.Categories = new SelectList(_sessionServices.GetCategoryForDropDownAsync().Result, "id", "CategoryName");
        return Task.CompletedTask;
    }
    private async Task PopulateTrainersAsync(CancellationToken ct)
    {
        ViewBag.Trainers = new SelectList(_sessionServices.GetTrainersForDropDownAsync().Result, "id", "Name");
    }
    [HttpGet]
    public async Task<IActionResult> Details(int id,CancellationToken ct )
    {
        var result=await _sessionServices.GetSessionlByIdAsync(id,ct);
        if (result.success) 
        {
            return View(result.value);
        }else
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }
        
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
    {
        var result = await _sessionServices.GetSessionToUpdateAsync(id, ct);
        if (!result.success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        await PopulateTrainersAsync(ct);
        return View(result.value);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]   
    public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)  
        {
            await PopulateTrainersAsync(ct);
            return View(model);
        }

        var result = await _sessionServices.UpdateSessionAsync(model, id, ct);

        if (result.success)
        {
            TempData["SuccessMessage"] = "Session updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        TempData["ErrorMessage"] = result.error;
        await PopulateTrainersAsync(ct);
        return View(model);
    }
    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
    {
        var result = await _sessionServices.GetSessionlByIdAsync(id, ct);

        if (!result.success)
        {
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        return View(result.value);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]        
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
    {
        var result = await _sessionServices.RemoveSessionAsync(id, ct);

        TempData[result.success ? "SuccessMessage" : "ErrorMessage"]
            = result.success ? "SessionDeleted" : result.error;

        return RedirectToAction(nameof(Index));
    }
}
 
