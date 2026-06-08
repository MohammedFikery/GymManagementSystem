using GymManagementSystem.DAL.Repository.Interfaces;
using GymManagementSystem.DbContexts;
using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repository.Classes
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        public PlanRepository(GymDbContext context) : base(context)
        {
        }
    }
}
