using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.Configurations
{
    public class PlanConfig : IEntityTypeConfiguration<Plan>
    {
        void IEntityTypeConfiguration<Plan>.Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(x => x.Name)
                   .HasColumnType("nvarchar")
                   .HasMaxLength(50);
            builder.Property(x => x.Description)
                   .HasMaxLength(200);
            builder.Property(x => x.Price)
                   .HasPrecision(10,2);
            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
            builder.ToTable(tb =>
            tb.HasCheckConstraint("CK_Plan_Duration", "Duration Between 1 and 365")
            );
        }
    }
}
