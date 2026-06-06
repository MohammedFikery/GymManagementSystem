using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Description).HasMaxLength(500);
            builder.Property(s => s.Capacity).IsRequired().HasDefaultValue(1);

            builder.Property(s => s.StartDate).IsRequired(); 
            builder.Property(s => s.EndDate).IsRequired();

            // Relationships
            builder.HasOne(s => s.Trainer)
                   .WithMany(t => t.Sessions)
                   .HasForeignKey(s => s.TrainerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Category)
                   .WithMany(c => c.Sessions)
                   .HasForeignKey(s => s.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Check Constraint الصحيح
            builder.ToTable(t => t.HasCheckConstraint("CK_Session_Dates", "[EndDate] > [StartDate]"));
        }
    }
}
