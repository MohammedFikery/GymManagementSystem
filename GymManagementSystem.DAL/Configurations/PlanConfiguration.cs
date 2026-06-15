using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasColumnType("Nvarchar")
                   .HasMaxLength(50);

            builder.Property(p => p.Description)
                   .HasMaxLength(200);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(p => p.DurationDays)
                   .IsRequired();

            builder.Property(p => p.IsActive)
                   .HasDefaultValue(true);
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationDaysCheck", "DurationDays Between 1 and 365");
            });
            builder.HasData(
        new Plan
        {
            Id = 1,
            Name = "Basic Monthly",
            Description = "Access to gym equipment during staffed hours",
            DurationDays = 1,
            Price = 300.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 2,
            Name = "Standard Monthly",
            Description = "Full gym access plus group classes",
            DurationDays = 30,
            Price = 500.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 3,
            Name = "Premium Monthly",
            Description = "Full access plus classes plus two PT sessions per month",
            DurationDays = 30,
            Price = 900.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 4,
            Name = "Quarterly Plan",
            Description = "Standard access for three months with discount",
            DurationDays = 90,
            Price = 1350.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 5,
            Name = "Semi-Annual Plan",
            Description = "Standard access for six months with bigger discount",
            DurationDays = 180,
            Price = 2500.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 6,
            Name = "Annual Plan",
            Description = "Full access for twelve months at best value",
            DurationDays = 365,
            Price = 4500.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 7,
            Name = "Student Plan",
            Description = "Discounted plan for students with valid ID",
            DurationDays = 30,
            Price = 250.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 8,
            Name = "Day Pass",
            Description = "Single-day access to gym facilities",
            DurationDays = 1,
            Price = 50.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 9,
            Name = "Old Trial Plan",
            Description = "Legacy trial plan no longer offered",
            DurationDays = 14,
            Price = 100.00m,
            IsActive = false,
            CreatedAt = new DateTime(2025, 6, 1),
            UpdatedAt = new DateTime(2026, 1, 1)
        },
        new Plan
        {
            Id = 10,
            Name = "VIP Plan",
            Description = "All access including sauna pool and personal locker",
            DurationDays = 30,
            Price = 1500.00m,
            IsActive = true,
            CreatedAt = new DateTime(2026, 2, 1)
        }
    );
        }
    }
}
