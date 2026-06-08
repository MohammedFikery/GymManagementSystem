using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .HasColumnName("StartDate");

            //builder.HasOne(m => m.Member)
            //       .WithMany(m => m.Memberships)
            //       .HasForeignKey(m => m.MemberId)
            //       .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(m => m.Plan)
            //       .WithMany(p => p.Memberships)
            //       .HasForeignKey(m => m.PlanId)
            //       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
