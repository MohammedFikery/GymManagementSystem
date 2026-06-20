using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
namespace GymManagementSystem.BLL.Services.Interfaces;

public interface ISessionServices
{
    Task<IEnumerable<SessionViewModel>?> GetAllSessionAsync(CancellationToken ct=default);
    Task<Result<SessionViewModel>> GetSessionlByIdAsync(int sessionid, CancellationToken ct = default);
    Task<Result> createSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
    Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct=default);
    Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsync(CancellationToken ct=default);
    Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId , CancellationToken ct = default);
    Task<Result> UpdateSessionAsync(UpdateSessionViewModel model, int sessionId, CancellationToken ct);
    Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct);

}
