using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using GymManagementSystem.DAL.UnitOFWork.Classes;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;

namespace GymManagementSystem.BLL.Services.Classes;

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
    public async Task<Result<SessionViewModel>> GetSessionlByIdAsync(int sessionid, CancellationToken ct = default)
    {
        var seison =await _unitOFWork.SessionRepository.GetSessionByIdwithTrainerAndCategery(sessionid,ct);
        if (seison==null)
        {
            return Result<SessionViewModel>.NotFound("Session Not Found");
        }
        else
        {
            var mappedSession = _mapper.Map<Session, SessionViewModel>(seison);
            mappedSession.AvailableSlots=mappedSession.Capacity-await _unitOFWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionid,ct);
            return Result<SessionViewModel>.OK(mappedSession);
        }
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
    public async Task<Result<UpdateSessionViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
    {
        var session=await _unitOFWork.SessionRepository.GetByIdAsync(sessionId,ct);
        if (session==null)
        {
            return Result<UpdateSessionViewModel>.NotFound("Session Not Fount");
        }
        if (session.StartDate>DateTime.UtcNow)
        {
            return Result<UpdateSessionViewModel>.Fail("can't update session has already start");
        }
        var bookingCount=await _unitOFWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId);
        if (bookingCount>0)
        {
            return Result<UpdateSessionViewModel>.Fail("can't update session has already has booking");
        }
        var mappedsession = _mapper.Map<UpdateSessionViewModel>(session);
        return Result<UpdateSessionViewModel>.OK(mappedsession);

    }
    public async Task<Result> UpdateSessionAsync(UpdateSessionViewModel model, int sessionId, CancellationToken ct)
    {
        var session = await _unitOFWork.SessionRepository.GetByIdAsync(sessionId,ct);
        if (session == null){return Result.NotFound("Session Not Fount");}
        if (session.StartDate <= DateTime.UtcNow) { return Result.Validation("Can't edit session that has already start"); }

        if (model.EndDate <= model.StartDate) { return Result.Validation("EndDate must be after StartDate"); }
        if (model.StartDate <= DateTime.UtcNow) { return Result.Validation("StartDate must be in the future."); }
        var bookingCount = await _unitOFWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId);
        if (bookingCount > 0){return Result.Fail("can't update session has already has booking");}


        var trainer = await _unitOFWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
        if (trainer is null) { return Result.NotFound("Trainer not found."); }
        var category = await _unitOFWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);
        if (category is null) { return Result.NotFound("Category not found."); }
        var isValid = Enum.TryParse<Specialties>(category.CategoryName, true, out var CategorySpecialty);
        if (!isValid || !trainer.Specialties.Contains(CategorySpecialty))
        { return Result.BusinessRule("Trainer does not have the required specialty for this category."); }
        _mapper.Map(model, session);
        session.UpdatedAt = DateTime.UtcNow;
        _unitOFWork.SessionRepository.Update(session);
        var rowEffected=await _unitOFWork.SaveChangesAsync();
        return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to update session");
    }
    public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct)
    {
        var session = await _unitOFWork.SessionRepository.GetByIdAsync(sessionId, ct);
        if (session == null) return Result.NotFound("Session is Not Found");
        if (session.EndDate >= DateTime.Now)
            return Result.Fail("Can Not Delete Session That Has Not Ended Yet");

        var BookedCount = await _unitOFWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
        if (BookedCount > 0)
            return Result.Fail("Can Not Delete Session That has Bookings");

        _unitOFWork.SessionRepository.Delete(session);
        var result = await _unitOFWork.SaveChangesAsync(ct);
        return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
    }
}
