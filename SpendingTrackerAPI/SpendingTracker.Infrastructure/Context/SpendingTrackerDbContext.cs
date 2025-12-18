using Microsoft.EntityFrameworkCore;
using SpendingTracker.Domain.Models;

namespace SpendingTracker.Infrastructure.Context
{
    public class SpendingTrackerDbContext : DbContext
    {
        private string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SpendingDb;Trusted_Connection=True;";
        
        public DbSet<User> Users { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundCategory> FundCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(eb =>
            {
                eb.Property(u => u.Email)
                    .HasMaxLength(50)
                    .IsRequired();

                eb.Property(u => u.Password)
                    .HasMaxLength(255)
                    .IsRequired();

                eb.Property(u => u.CreationDate)
                    .HasDefaultValueSql("getutcdate()");

                eb.HasMany(u => u.Funds)
                  .WithOne(f => f.User)
                  .HasForeignKey(f => f.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

                eb.HasMany(u => u.FundCategories)
                  .WithOne(fc => fc.User)
                  .HasForeignKey(fc => fc.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Fund>(eb =>
            {
                eb.Property(f => f.Amount)
                    .HasPrecision(15, 2);

                eb.Property(f => f.CreationDate)
                    .HasDefaultValueSql("getutcdate()");

                eb.HasOne(f => f.FundCategory)
                  .WithMany(fc => fc.Funds)
                  .HasForeignKey(f => f.FundCategoryId)
                  .OnDelete(DeleteBehavior.SetNull);
            });


            modelBuilder.Entity<FundCategory>(eb =>
            {
                eb.Property(fc => fc.ShouldBe)
                    .HasPrecision(15, 2);

                eb.Property(fc => fc.Name)
                .HasMaxLength(30);

                    eb.Property(u => u.CreationDate)
                    .HasDefaultValueSql("getutcdate()");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
