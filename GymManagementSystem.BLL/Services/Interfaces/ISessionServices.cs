using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
        Task<IEnumerable<SessionViewModel>?> GetAllSessionAsync(CancellationToken ct=default);

        Task<Result> createSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct=default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsync(CancellationToken ct=default);
    }
}
