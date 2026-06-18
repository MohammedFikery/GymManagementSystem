using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionServices _sessionServices;

        public SessionController(ISessionServices sessionServices)
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
}
}
