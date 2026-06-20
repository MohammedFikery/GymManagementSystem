using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.HealthRecordViewModels;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct=default);

        Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<Result> UpdateMemberAsync(int memberId,MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<Result> DeleteMemberAsync(int memberId, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailesByIdAsync(int memberId, CancellationToken ct);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct);
    }
}
 