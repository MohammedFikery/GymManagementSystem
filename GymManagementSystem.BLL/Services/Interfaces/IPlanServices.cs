using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IPlanServices
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct=default);
        Task<PlanViewModel?> GetPlanByIdAsync(int PlanID,CancellationToken ct=default);
        Task<UpdatePlaneViewModel?> GetPlaneToUpdate(int PlanID,CancellationToken ct = default);
        Task<bool> upatePlanAsync(int id,UpdatePlaneViewModel model, CancellationToken ct = default);
        Task<bool> ToggleActivetedAsync(int PlanID, CancellationToken ct=default);
    }
}
