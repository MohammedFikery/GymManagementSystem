using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; } = 1;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
