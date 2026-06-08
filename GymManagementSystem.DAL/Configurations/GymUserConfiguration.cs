using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T>
           where T : GymUser
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Phone)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.Property(x => x.DateOfBirth)
                   .IsRequired();

            builder.Property(x => x.Gender)
                   .IsRequired()
                   .HasConversion<string>();

            // Owned Entity: Address
            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.BuildingNumber).HasMaxLength(20);
                address.Property(a => a.Street).HasMaxLength(30);
                address.Property(a => a.City).HasMaxLength(30);
            });

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Phone).IsUnique();
        }
    }
}
