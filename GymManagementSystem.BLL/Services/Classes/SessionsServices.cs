using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.Repository.Classes;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DAL.UnitOFWork.Classes;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionsServices : ISessionServices
    {

        #region Repositories
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor
        public SessionsServices(IUnitOFWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }
        #endregion
        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionAsync(CancellationToken ct = default)
        {
            var sessionRepository = _unitOFWork.SessionRepository;

            var sessions =await sessionRepository.GetAllSessionswithTrainerAndCategery(ct);

            if (sessions == null || !sessions.Any())return Enumerable.Empty<SessionViewModel>();

            var result =_mapper.Map<List<SessionViewModel>>(sessions);

            var sessionIds =result.Select(x => x.Id);

            var bookedSlotsDictionary =await sessionRepository.GetBookedSlotsCountAsync(sessionIds, ct);

            foreach (var session in result)
            {
                bookedSlotsDictionary.TryGetValue(session.Id,out var bookedCount);
                session.AvailableSlots =Math.Max(0,session.Capacity - bookedCount);
            }

            return result;
        }

        public async Task<Result> createSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate must be after StartDate");
            if (model.StartDate <= DateTime.UtcNow) return Result.Validation("StartDate must be in the future.");
            if (model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity must be between 1 and 25");
            var trainer = await _unitOFWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer is null) return Result.NotFound("Trainer not found.");
            var category = await _unitOFWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if (category is null) return Result.NotFound("Category not found.");
            var isValid = Enum.TryParse<Specialties>(category.CategoryName, true, out var CategorySpecialty);
            if (!isValid || !trainer.Specialties.Contains(CategorySpecialty)) return Result.BusinessRule("Trainer does not have the required specialty for this category.");
            var session = _mapper.Map<Session>(model);
            _unitOFWork.GetRepository<Session>().Add(session);
            var affected = await _unitOFWork.SaveChangesAsync(ct);
            return affected > 0 ? Result.Ok(): Result.Fail("Failed to persist the session.", ResultKind.Failure);

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
           var trainerRepository = await _unitOFWork.GetRepository<Trainer>().GetAllAsync(ct:ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainerRepository);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoryForDropDownAsync(CancellationToken ct = default)
        {
            var categoryRepository =await _unitOFWork.GetRepository<Category>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categoryRepository);
        }
    }
}
