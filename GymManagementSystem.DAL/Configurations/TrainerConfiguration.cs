using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.HasBaseType<GymUser>();

            builder.Property(t => t.Specialties)
                   .HasConversion(
                       v => string.Join(",", v.Select(s => s.ToString())),
                       v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(s => Enum.Parse<Specialty>(s))
                             .ToList()
                   );
        }
    }
}
