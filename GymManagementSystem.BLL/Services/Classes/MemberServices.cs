using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberServices(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);
            if (!members.Any()) return [];
            var membersViewModel = members.Select(m => new MemberViewModel
            {
                Email = m.Email,
                Gender = m.Gender.ToString(),
                Id = m.Id,
                Name = m.Name,
                Phone = m.Phone,
                Photo = m.Photo

            });
            return membersViewModel;


        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _memberRepository.AnyAsync(m=>m.Email==model.Email,ct);
            var PhoneExists = await _memberRepository.AnyAsync(m=>m.Phone==model.Phone,ct);
            if (emailExists || PhoneExists) return false;

            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateOfBirth =model.DateOfBirth,     
                Gender = model.Gender,

                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber.ToString(),
                    Street = model.Street,
                    City = model.City
                },

                HealthRecord = new HealthRecord
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note
                }
            };
            var rowsAffected = await _memberRepository.AddAsync(member, ct);
            return rowsAffected > 0;

          }
        public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
          
            var count= await _memberRepository.DeleteAsync(id, ct);
            return (count > 0);
           
        }
        public Task<MemberViewModel?> GetMemberDetailesByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateMemberAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

    }
}
