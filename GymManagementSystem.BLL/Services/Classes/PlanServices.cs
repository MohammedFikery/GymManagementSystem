using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.PlanViewModel;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Classes;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DAL.UnitOFWork.Classes;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using GymManagementSystem.Models;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOFWork _unitOFWork;

        public PlanServices(IUnitOFWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }

        public IGenericRepository<Membership> IMemberShipRepository { get; }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOFWork.GetRepository<Plan>().GetAllAsync(ct: ct);

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                Price = p.Price,
                IsActive = p.IsActive
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int PlanID, CancellationToken ct = default)
        {
            var plan = await _unitOFWork.GetRepository<Plan>().GetByIdAsync(PlanID, ct);
            if (plan is null) return null;
            else

                return new PlanViewModel
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Description = plan.Description,
                    DurationDays = plan.DurationDays,
                    Price = plan.Price,
                    IsActive = plan.IsActive
                };
        }

        public async Task<UpdatePlaneViewModel?> GetPlaneToUpdate(int PlanID, CancellationToken ct = default)
        {
            var plan = await _unitOFWork.GetRepository<Plan>().GetByIdAsync(PlanID, ct);
            if (plan is null || plan.IsActive) return null;
            if (await HasAcctiveMemberShipAsync(PlanID, ct))
                return null;
            else
                return new UpdatePlaneViewModel
                {
                    PlanName = plan.Name,
                    Description = plan.Description ?? "",
                    DurationDays = plan.DurationDays,
                    Prlice = plan.Price
                };
        }

        public Task<bool> ToggleActivetedAsync(int PlanID, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> upatePlanAsync(int id, UpdatePlaneViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOFWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return false;
            if (await HasAcctiveMemberShipAsync(id, ct))
                return false;
            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Prlice;
            plan.UpdatedAt = DateTime.Now;
            _unitOFWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOFWork.SaveChangesAsync();
            return result > 0;
        }

        private async Task<bool> HasAcctiveMemberShipAsync(int PlanID, CancellationToken ct = default)
        {
            return await _unitOFWork.GetRepository<Membership>().AnyAsync(m => m.PlanId == PlanID && m.EndDate > DateTime.Now, ct);

        }
    }
}
