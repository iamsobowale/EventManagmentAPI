using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class ApplicationContext: DbContext
{
    
    public ApplicationContext(DbContextOptions options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Wallet)
            .WithOne(w => w.User)
            .HasForeignKey<Wallet>(w => w.UserId);
        var johnUserId = Guid.NewGuid();
        var janeUserId = Guid.NewGuid();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = johnUserId,
                Username = "john_doe",
                Email = "john@example.com",
                Role = "Admin",
                Gender = "Male",
                Password = "1234"
            },
            new User
            {
                Id = janeUserId,
                Username = "jane_doe",
                Email = "jane@example.com",
                Role = "User",
                Gender = "Female",
                Password = "5678"
            }
        );
        
        modelBuilder.Entity<Wallet>().HasData(
            new Wallet
            {
                Id = Guid.NewGuid(),
                Balance = 0,
                UserId = johnUserId
            },
            new Wallet
            {
                Id = Guid.NewGuid(),
                Balance = 0,
                UserId = janeUserId
            }
        );
    }
    
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
}