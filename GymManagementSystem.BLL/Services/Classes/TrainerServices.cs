using AutoMapper;
using AutoMapper.Execution;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.BLL.ViewModels.Trainer;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DAL.UnitOFWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class TrainerServices : ITrainerServices
    {
        #region Repositories
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constractor
        public TrainerServices(IUnitOFWork unitOFWork, IMapper mapper)
        {
            _unitOFWork = unitOFWork;
            _mapper = mapper;
        }
        #endregion
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainerAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOFWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            if (!trainers.Any()) return [];
            var TranierViewModel = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
            return TranierViewModel;
        }
        public async Task<TrainerViewModel?> GetTrainerDetailesByIdAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOFWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null)
                return null;

            return _mapper.Map<TrainerViewModel>(trainer);
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOFWork.GetRepository<Trainer>().AnyAsync(m => m.Email == model.Email, ct);
            var PhoneExists = await _unitOFWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (emailExists || PhoneExists) return false;
            var trainer = _mapper.Map<Trainer>(model);
            _unitOFWork.GetRepository<Trainer>().Add(trainer);
            var rowsAffected = await _unitOFWork.SaveChangesAsync(ct);
            return rowsAffected > 0;
        }
        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOFWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer == null) return null;
            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }
        public async Task<bool> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOFWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null)return false;
            if (await _unitOFWork.GetRepository<Trainer>().AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct)) return false;
            if (await _unitOFWork.GetRepository<Trainer>().AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct)) return false;
            _mapper.Map(model, trainer);
            trainer.UpdatedAt = DateTime.Now;
            var rowsAffected = await _unitOFWork.SaveChangesAsync(ct);
            return rowsAffected > 0;
        }
        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _unitOFWork.GetRepository<Trainer>().GetByIdAsync(trainerId, ct);
            if (trainer is null)return false;
            var hasFutureSessions = await _unitOFWork.GetRepository<Session>().AnyAsync(s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);
            if (hasFutureSessions)return false;
            _unitOFWork.GetRepository<Trainer>().Delete(trainer);
            var count = await _unitOFWork.SaveChangesAsync(ct);
            return (count > 0);
        }
    }
}
