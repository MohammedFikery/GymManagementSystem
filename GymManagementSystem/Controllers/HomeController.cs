using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagementSystem.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly IAnalyticsServices _analyticsServices;

    public HomeController(IAnalyticsServices analyticsServices)
    {
        _analyticsServices = analyticsServices;
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var data = await _analyticsServices.GetDataAsync(ct);
        return View(data);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
