using AutoMapper;
using GymManagementSystem.BLL.AttachmentServices;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.HealthRecordViewModels;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using Microsoft.Extensions.Logging;


namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberService
    {

        #region Repositories
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentServices _attachmentServices;
        private readonly ILogger<MemberServices> _logger;
        #endregion
        #region Constractor
        public MemberServices(IUnitOFWork unitOFWork,IMapper mapper,IAttachmentServices attachmentServices,ILogger<MemberServices> logger)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
            _attachmentServices = attachmentServices;
            _logger = logger;
        }
        #endregion

        #region Tasks
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOFWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];
            var membersViewModel =_mapper.Map<IEnumerable<MemberViewModel>>(members);
            return membersViewModel;
        }
        public async Task<MemberViewModel?> GetMemberDetailesByIdAsync(int memberID, CancellationToken ct)
        {
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(memberID, ct);
            if (member == null) return null;
            var model = _mapper.Map<MemberViewModel>(member);
            var activeMembership = await _unitOFWork.GetRepository<Membership>().FirstOrDefaultAsync(m => m.MemberId == memberID && m.EndDate > DateTime.Now);
            if (activeMembership is not null)
            {
                model.PlanName = activeMembership.Plan.Name;
                model.MembershipStartDate = activeMembership.CreatedAt.ToString("yyyy-MM-dd");
                model.MembershipEndDate = activeMembership.EndDate.ToString("yyyy-MM-dd");  
            }
            return model;
        }
        public async Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            if (model.PhotoFile is null || model.PhotoFile.Length == 0)
                return Result.Validation("A member photo is required.");

            var emailExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            if (emailExists)return Result.Fail("This email is already registered.");

            var phoneExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (phoneExists)return Result.Fail("This phone number is already registered.");

            string storedPhotoName;
            await using (var photoStream = model.PhotoFile.OpenReadStream())
            {
                var uploadResult = await _attachmentServices.UploadAsync(photoStream, model.PhotoFile.FileName, "MembersPhotos", ct);

                if (!uploadResult.IsSuccess)
                {
                    _logger.LogWarning("Member photo upload failed: {Error}", uploadResult.Error);
                    return Result.Fail("Failed to upload member photo.");
                }
                storedPhotoName = uploadResult.StoredFileName!;
            }

            try
            {
                var member = _mapper.Map<Member>(model);
                member.Photo = storedPhotoName;

                _unitOFWork.GetRepository<Member>().Add(member);
                var rowsAffected = await _unitOFWork.SaveChangesAsync(ct);

                if (rowsAffected > 0)
                    return Result.Ok();

                await _attachmentServices.DeleteAsync(storedPhotoName, "MembersPhotos", ct);
                return Result.Fail("Failed to save the member. Please try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create member. Cleaning up photo {Photo}", storedPhotoName);
                await _attachmentServices.DeleteAsync(storedPhotoName, "MembersPhotos", ct);
                throw;
            }
        }
        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct)
        {
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null) return null;
            var model = _mapper.Map<MemberToUpdateViewModel>(member);
            return model;

        }
        public async Task<Result> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            if (emailExists) return Result.Fail("This email is already registered.");

            var phoneExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);
            if (phoneExists) return Result.Fail("This phone number is already registered.");

            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return Result.NotFound("Member not found.");

            _mapper.Map(model, member);
            member.UpdatedAt = DateTime.UtcNow;

            _unitOFWork.GetRepository<Member>().Update(member);
            var rowsAffected = await _unitOFWork.SaveChangesAsync(ct);

            return rowsAffected > 0
                ? Result.Ok()
                : Result.Fail("Failed to update the member.");
        }
        public async Task<Result> DeleteMemberAsync(int memberID, CancellationToken ct = default)
        {
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(memberID, ct);
            if (member is null) return Result.NotFound("Member not found.");

            var now = DateTime.UtcNow;
            var hasActiveBookings = await _unitOFWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberID && b.Session.StartDate > now, ct);

            if (hasActiveBookings) return Result.BusinessRule("Cannot delete a member with active bookings.");

            _unitOFWork.GetRepository<Member>().Delete(member);
            var count = await _unitOFWork.SaveChangesAsync(ct);

            return count > 0
                ? Result.Ok()
                : Result.Fail("Failed to delete the member.");
        }
        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await _unitOFWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, ct);

            if (record is null) return null;
            var model = _mapper.Map<HealthRecordViewModel>(record);
            return model;
        } 
        #endregion

    }
}
