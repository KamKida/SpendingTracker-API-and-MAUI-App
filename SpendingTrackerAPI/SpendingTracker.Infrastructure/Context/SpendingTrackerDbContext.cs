using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain.Models;

namespace SpendingTracker.Infrastructure.Context
{
    public class SpendingTrackerDbContext : DbContext
    {
        private string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SpendingDb;Trusted_Connection=True;";
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.CreationDate)
                .HasDefaultValueSql("getutcdate()");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
