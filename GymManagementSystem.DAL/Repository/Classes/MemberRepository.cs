using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repository.Classes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbContext context) : base(context)
        {
        }
    }
}
