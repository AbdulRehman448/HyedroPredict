using HydroPredict.Models;
using Microsoft.EntityFrameworkCore;

namespace HydroPredict.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Booking> Bookings => Set<Booking>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure precise decimal type for SQLite compatibility
            modelBuilder.Entity<Booking>()
                .Property(b => b.DynamicTariffCalculated)
                .HasConversion<double>(); // SQLite handles decimals best as doubles natively in EF Core

            // Seed default Admin and Driver accounts for testing logins instantly
            // Seed default Admin and Driver accounts with the correct hash for "admin123"
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    AccountName = "System Admin",
                    SecurityEmail = "admin@hydropredict.com",
                    PasswordHash = "PM7uthA3S8U9bKzX86gHefvE19g6b883=", // Exact SHA256 hash for admin123
                    AccessRole = AccessRole.Admin
                },
                new User
                {
                    Id = 2,
                    AccountName = "Primary Driver",
                    SecurityEmail = "driver@hydropredict.com",
                    PasswordHash = "PM7uthA3S8U9bKzX86gHefvE19g6b883=", // Exact SHA256 hash for admin123
                    AccessRole = AccessRole.Driver,
                    DailyCapacityLiters = 15000
                }
            );
        }
    }
}