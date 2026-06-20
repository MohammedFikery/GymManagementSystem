using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes;

public sealed class PlanServices : IPlanServices
{
    private readonly IUnitOFWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlanServices(IUnitOFWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
    {
        var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
        return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
    }

    public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
        return plan is null ? null : _mapper.Map<PlanViewModel>(plan);
    }

    public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
        if (plan is null) return null;

        if (await HasActiveMembershipsAsync(planId, ct))
            return null;

        return _mapper.Map<UpdatePlanViewModel>(plan);
    }

    public async Task<bool> UpdatePlanAsync(int planId, UpdatePlanViewModel model, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
        if (plan is null) return false;

        if (await HasActiveMembershipsAsync(planId, ct))
            return false;

        _mapper.Map(model, plan);
        plan.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.GetRepository<Plan>().Update(plan);
        var affected = await _unitOfWork.SaveChangesAsync(ct); // تمرير الـ ct
        return affected > 0;
    }

    public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default)
    {
        var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
        if (plan is null) return false;

        if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
            return false;

        plan.IsActive = !plan.IsActive;
        plan.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.GetRepository<Plan>().Update(plan);
        var affected = await _unitOfWork.SaveChangesAsync(ct);
        return affected > 0;
    }

    private async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct = default)
        => await _unitOfWork.GetRepository<Membership>()
                            .AnyAsync(m => m.PlanId == planId && m.EndDate > DateTime.UtcNow, ct);
}
