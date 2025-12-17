using Microsoft.EntityFrameworkCore;
using SpendingsManagementWebAPI.Entities;

namespace SpendingsManagementWebAPI
{
    public class SpendingDbContext : DbContext
    {
        private string _connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SpendingDb;Trusted_Connection=True;";

        public DbSet<User> Users { get; set; } 
        public DbSet<AddedFund> AddedFunds { get; set; }
        public DbSet<Group> SpendingGroups { get; set; }
        public DbSet<GroupLimit> GroupLimits { get; set; }
        public DbSet<PlannedSpending> PlanedSpendings { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<SpendingLimit> SpendingLimits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();

            modelBuilder.Entity<AddedFund>()
                .HasOne(f => f.User)
                .WithMany(u => u.Funds)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddedFund>()
                .Property(f => f.AmountAdded)
                .IsRequired();

            modelBuilder.Entity<AddedFund>()
                .Property(f => f.Currency)
                .IsRequired();

            modelBuilder.Entity<AddedFund>()
                .Property(f => f.Source)
                .HasMaxLength(20);

            modelBuilder.Entity<Group>()
                .HasOne(g => g.User)
                .WithMany(g => g.Groups)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Group>()
                .Property(g => g.Name)
                .HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<GroupLimit>()
                .HasOne(l => l.Group)
                .WithOne(g => g.GroupLimit)
                .HasForeignKey<GroupLimit>(g => g.GroupId);

            modelBuilder.Entity<Spending>()
                .HasOne(s => s.User)
                .WithMany(u => u.Spendings)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Spending>()
                .Property(s => s.Amount)
                .IsRequired();

            modelBuilder.Entity<SpendingLimit>()
                .HasOne(l => l.User)
                .WithMany(u => u.SpendingLimits)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SpendingLimit>()
                .Property(s => s.Limit)
                .IsRequired();

            modelBuilder.Entity<SpendingLimit>()
                .Property(s => s.NumberOfDays)
                .IsRequired();

            modelBuilder.Entity<PlannedSpending>()
                .HasOne(s => s.User)
                .WithMany(u => u.PlanedSpendings)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlannedSpending>()
                .HasOne(s => s.Group)
                .WithOne(g => g.PlannedSpending)
                .HasForeignKey<PlannedSpending>(s => s.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
