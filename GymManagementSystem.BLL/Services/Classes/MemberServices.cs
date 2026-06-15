using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.HealthRecordViewModels;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;


namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberService
    {

        #region Repositories
        private readonly IUnitOFWork _unitOFWork;
        #endregion

        #region Constractor
        public MemberServices(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }
        #endregion

        #region Tasks
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOFWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];
            var membersViewModel = members.Select(m => new MemberViewModel
            {
                Email = m.Email,
                Gender = m.Gender.ToString(),
                Id = m.Id,
                Name = m.Name,
                Phone = m.Phone,
                Photo = m.Photo

            });
            return membersViewModel;


        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var PhoneExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (emailExists || PhoneExists) return false;

            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,

                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber.ToString(),
                    Street = model.Street,
                    City = model.City
                },

                HealthRecord = new HealthRecord
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };
            _unitOFWork.GetRepository<Member>().Add(member);
            var rowsAffected =await _unitOFWork.SaveChangesAsync(ct);
            return rowsAffected > 0;

        }
        public async Task<bool> DeleteMemberAsync(int memberID, CancellationToken ct = default)
        {
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(memberID, ct);
            if (member is null) return false;
            var hasActiveBookings = await _unitOFWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberID && b.Session.StartDate > DateTime.Now, ct);
            if (hasActiveBookings) return false;
             _unitOFWork.GetRepository<Member>().Delete(member);
            var count = await _unitOFWork.SaveChangesAsync(ct);
            return (count > 0);
        }
        public async Task<MemberViewModel?> GetMemberDetailesByIdAsync(int memberID, CancellationToken ct)
        {
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(memberID, ct);
            if (member == null) return null;
            var model = new MemberViewModel()
            {
                Email = member.Email,
                Photo = member.Photo,
                Name = member.Name,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToString("yyyy-MM-dd"),
                Address = $"{member.Address.BuildingNumber}-{member.Address.Street}-{member.Address.City}",
            };
            var activeMembership = await _unitOFWork.GetRepository<Membership>().FirstOrDefaultAsync(m => m.MemberId == memberID && m.EndDate > DateTime.Now);
            if (activeMembership is not null)
            {
                model.PlanName = activeMembership.Plan.Name;
                model.MembershipStartDate = activeMembership.CreatedAt.ToString("yyyy-MM-dd");
                model.MembershipEndDate = activeMembership.EndDate.ToString("yyyy-MM-dd");
            }
            return model;
        }
        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct)
        {
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (member is null) return null;
            var model = new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                BuildingNumber = int.Parse(member.Address.BuildingNumber),
                Street = member.Address.Street,
                City = member.Address.City
            };
            return model;

        }
        public async Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            var PhoneExists = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);
            if (emailExists || PhoneExists) return false;

            var member = await _unitOFWork.GetRepository<Member>().GetByIdAsync(memberId, ct);
            if (member is null) return false;

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Name = model.Name;
            member.Address.BuildingNumber = model.BuildingNumber.ToString();
            member.Address.Street = model.Street;
            member.Address.City = model.City;

            _unitOFWork.GetRepository<Member>().Update(member);
            var rowsAffected = await _unitOFWork.SaveChangesAsync(ct);
            return rowsAffected > 0;

        }
        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var record = await _unitOFWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, ct);

            if (record is null) return null;
            var model = new HealthRecordViewModel()
            {
                Height = record.Height,
                Weight = record.Weight,
                BloodType = record.BloodType,
                Note = record.Note
            };
            return model;
        } 
        #endregion

    }
}
