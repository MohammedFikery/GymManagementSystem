using GymManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class Membership : BaseEntity
    {
        public DateTime EndDate { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
        [NotMapped]
        public bool IsActive => EndDate > DateTime.Now;
        public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
    }
}
 