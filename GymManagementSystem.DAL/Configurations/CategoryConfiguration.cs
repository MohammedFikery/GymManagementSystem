using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CategoryName)
                   .IsRequired()
                   .HasColumnType("Nvarchar")
                   .HasMaxLength(20);

            builder.HasIndex(c => c.CategoryName).IsUnique();
            builder.HasData(
                            new Category { Id = 1, CategoryName = "Cardio" },
                            new Category { Id = 2, CategoryName = "Strength" },
                            new Category { Id = 3, CategoryName = "Yoga" },
                            new Category { Id = 4, CategoryName = "Boxing" },
                            new Category { Id = 5, CategoryName = "CrossFit" }
                            );
        }
    }
}
