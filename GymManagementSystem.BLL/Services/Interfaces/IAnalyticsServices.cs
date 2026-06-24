
using GymManagementSystem.BLL.ViewModels.AnalyticsViewModels;

namespace GymManagementSystem.BLL.Services.Interfaces;

public interface IAnalyticsServices
{
    Task<AnalyticsViewModel> GetDataAsync(CancellationToken cancellationToken = default);
}
