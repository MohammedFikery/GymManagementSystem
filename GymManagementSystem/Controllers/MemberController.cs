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
        public async Task<IActionResult> Index(CancellationToken ct)
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
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.CreateMemberAsync(model, ct);
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
        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var result = await _memberService.GetMemberDetailesByIdAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Member not found...!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var result = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "HealthRecord not found...!!!";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found...!!!";
                return RedirectToAction(nameof(Index));
            }

            var model = new MemberToUpdateViewModel
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.BuildingNumber,
                City = member.City,
                Street = member.Street
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberAsync(id, model, ct);
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _memberService.GetMemberDetailesByIdAsync(id, ct);
            if (result == null)
            {
                TempData["Error Message"] = "Member not found...!!!";
                return RedirectToAction(nameof(Index));
            }

            return View();

        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = _memberService.DeleteMemberAsync(id, ct);
            if (result.Result)
            {
                TempData["SuccessMessage"] = "Member deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete member. Please try again.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}