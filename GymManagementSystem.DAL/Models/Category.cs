using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementSystem.DAL.Models
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
