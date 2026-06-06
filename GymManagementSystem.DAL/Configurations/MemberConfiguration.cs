using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasBaseType<GymUser>(); 

            builder.Property(m => m.JoinDate)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(m => m.Photo)
                   .HasMaxLength(500);

            // Relationships
            builder.HasOne(m => m.HealthRecord)
                   .WithOne(h => h.Member)
                   .HasForeignKey<HealthRecord>(h => h.MemberId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
