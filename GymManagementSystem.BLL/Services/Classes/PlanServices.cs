using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Classes;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DAL.UnitOFWork.Classes;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using GymManagementSystem.Models;
using System.Numerics;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;

        public PlanServices(IUnitOFWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }
        public IGenericRepository<Membership> IMemberShipRepository { get; }
        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOFWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }
        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanID, CancellationToken ct = default)
        {
            var plan = await _unitOFWork.GetRepository<Plan>().GetByIdAsync(PlanID, ct);
            if (plan is null) return null;
            return _mapper.Map<PlanViewModel>(plan);
        }
        public async Task<UpdatePlaneViewModel?> GetPlaneToUpdate(int PlanID, CancellationToken ct = default)
        {
            var plan = await _unitOFWork.GetRepository<Plan>().GetByIdAsync(PlanID, ct);
            if (plan is null || plan.IsActive) return null;
            if (await HasActiveMembershipsAsync(PlanID, ct))
                return null;
            else
                return _mapper.Map<UpdatePlaneViewModel>(plan);
        }
        public async Task<bool> ToggleActivetedAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _unitOFWork.GetRepository<Plan>().GetByIdAsync(planId, ct);
            if (plan is null) return false;

            if (plan.IsActive && await HasActiveMembershipsAsync(planId, ct))
                return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;
            _unitOFWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOFWork.SaveChangesAsync(ct);
            return result > 0;
        }
        public async Task<bool> upatePlanAsync(int id, UpdatePlaneViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOFWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return false;
            if (await HasActiveMembershipsAsync(id, ct))
                return false;
            _mapper.Map(model, plan);
            plan.UpdatedAt = DateTime.Now;
            _unitOFWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOFWork.SaveChangesAsync();
            return result > 0;
        }
        private async Task<bool> HasActiveMembershipsAsync(int PlanID, CancellationToken ct = default)
        {
            return await _unitOFWork.GetRepository<Membership>().AnyAsync(m => m.PlanId == PlanID && m.EndDate > DateTime.Now, ct);

        }
    }
}
