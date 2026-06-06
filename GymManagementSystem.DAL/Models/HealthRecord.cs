using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public int MemberId { get; set; }

        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string? BloodType { get; set; }
        public string? Note { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public Member Member { get; set; } = null!;
    }
}
