using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class MemberConfiguration : GymUserConfiguration<Member>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder); 

            builder.Property(m => m.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.Property(m => m.Photo)
                   .HasMaxLength(500);
        }
    }
}
