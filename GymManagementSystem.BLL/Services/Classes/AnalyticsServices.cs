
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
namespace GymManagementSystem.BLL.Services.Classes;

public class AnalyticsServices : IAnalyticsServices
{
    private readonly IUnitOFWork _unitOfWork;

    public AnalyticsServices(IUnitOFWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<AnalyticsViewModel> GetDataAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var upcomingSessions = await _unitOfWork.GetRepository<Session>().GetCountAsync(s => s.StartDate > now, ct);
        var ongoingSessions = await _unitOfWork.GetRepository<Session>().GetCountAsync(s => s.StartDate <= now && s.EndDate >= now, ct);
        var completedSessions = await _unitOfWork.GetRepository<Session>().GetCountAsync(s => s.EndDate < now, ct);
        var totalMembers = await _unitOfWork.GetRepository<Member>().GetCountAsync(ct: ct);
        var totalTrainers = await _unitOfWork.GetRepository<Trainer>().GetCountAsync(ct: ct);
        var activeMembers = await _unitOfWork.GetRepository<Membership>().GetCountAsync(x => x.EndDate > now, ct);

        return new AnalyticsViewModel
        {
            TotalMembers = totalMembers,
            ActiveMembers = activeMembers,
            TotalTrainers = totalTrainers,
            UpcomingSessions = upcomingSessions,
            OngoingSessions = ongoingSessions,
            CompletedSessions = completedSessions
        };
    }
}
