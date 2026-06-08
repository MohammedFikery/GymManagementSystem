using GymManagementSystem.BLL.Repositories;
using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Repository.Interfaces
{
    public  interface IPlanRepository 
    {
        Task <IEnumerable<Plan>> GetAllAsync(bool tracking =false ,CancellationToken ct=default);
        Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> AddAsync(Plan plan, CancellationToken ct = default);
        Task UpdateAsync(Plan plan, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);


    }
}
