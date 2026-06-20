using GymManagementSystem.BLL.AttachmentServices;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentServices _attachmentServices;

        public MemberController(IMemberService memberService, IAttachmentServices attachmentServices)
        {
            _memberService = memberService;
            _attachmentServices = attachmentServices;
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
            if (!ModelState.IsValid)
                return View(model);

            var result = await _memberService.CreateMemberAsync(model, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Member created successfully!";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
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
            if (!ModelState.IsValid) return View(model); 
            var result = await _memberService.UpdateMemberAsync(id, model, ct);
            if (result.success)
                TempData["SuccessMessage"] = "Member Updated successfully!";
            else
                TempData["ErrorMessage"] = result.error;

            return RedirectToAction(nameof(Index));
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

            return View(result);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);

            if (result.success)
                TempData["SuccessMessage"] = "Member deleted successfully!";
            else
                TempData["ErrorMessage"] = result.error; // هيظهر "Cannot delete..." بدل رسالة عامة

            return RedirectToAction(nameof(Index));
        }
        [HttpGet("member-photo/{fileName}")]
        public IActionResult GetMemberPhoto(string fileName)
        {
            var file = _attachmentServices.GetFile(fileName, "MembersPhotos");
            if (file is null) return NotFound();
            return File(file.Value.stream, file.Value.contentType);
        }
    }
}