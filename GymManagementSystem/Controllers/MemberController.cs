using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;   

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateMemberViewModel(); 
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var result = await _memberService.CreateMemberAsync(model, ct);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Failed to create member. Please try again.");
            return View(model);
        }

        public IActionResult HealthRecordDetails(int id, CancellationToken ct = default)
        {
            return View();
        }
    }
}