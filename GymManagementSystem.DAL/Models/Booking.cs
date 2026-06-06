using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class Booking : BaseEntity
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public bool IsAttended { get; set; } = false;

        // Navigation Properties
        public Member Member { get; set; } = null!;
        public Session Session { get; set; } = null!;
    }
}
