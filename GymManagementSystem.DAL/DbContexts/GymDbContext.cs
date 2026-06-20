
using GymManagementSystem.DAL.Configurations;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DbContexts
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        #region DbSets
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MemberConfiguration());
            modelBuilder.ApplyConfiguration(new TrainerConfiguration());
            modelBuilder.ApplyConfiguration(new PlanConfiguration());
            modelBuilder.ApplyConfiguration(new HealthRecordConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new SessionConfiguration());
            modelBuilder.ApplyConfiguration(new MembershipConfiguration());
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
            modelBuilder.Entity<Session>().ToTable(t => t.HasCheckConstraint("CK_Session_Dates", "[EndDate] > [StartDate]"));

            modelBuilder.Entity<Session>()
                .Property(s => s.Capacity)
                .HasDefaultValue(1);
        }
    }
}
