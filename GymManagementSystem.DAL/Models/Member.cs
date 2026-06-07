using GymManagementSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class Member : GymUser
    {
        [MaxLength(500)]
        public string? Photo { get; set; }
        public HealthRecord? HealthRecord { get; set; }

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
