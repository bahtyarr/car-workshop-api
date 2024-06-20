using CarWorkshopSystem.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshopSystem.Infrastructure
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }

        public DbSet<CarOwner> CarOwners { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Car)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.CarId);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Service)
                .WithMany(s => s.Jobs)
                .HasForeignKey(j => j.ServiceId);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Mechanic)
                .WithMany(m => m.Jobs)
                .HasForeignKey(j => j.MechanicId);

            modelBuilder.Entity<Repair>()
            .HasOne(r => r.Car)
            .WithMany(c => c.Repairs)
            .HasForeignKey(r => r.CarId);

            base.OnModelCreating(modelBuilder);
        }
    }
}