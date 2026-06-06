using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementSystem.DAL.Models
{
    public class Trainer : GymUser
    {
        [MaxLength(200)]
        public List<Specialty> Specialties { get; set; } = new List<Specialty>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
