using GymManagementSystem.BLL.ViewModels.HealthRecordViewModels;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct=default);

        Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<bool> UpdateMemberAsync(int memberId,MemberToUpdateViewModel model, CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default);
        Task<MemberViewModel?> GetMemberDetailesByIdAsync(int id, CancellationToken ct);
        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct);
        Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct);
    }
}
 