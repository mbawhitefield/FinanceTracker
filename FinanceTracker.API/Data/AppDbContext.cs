using FinanceTracker.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Data;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Required for identity tables to be created

        // Configure the transaction entity
        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Amount)
            .HasColumnType("decimal(18, 2)"); // Specify precision and scale for decimal type

            entity.Property(t => t.Type)
            .HasMaxLength(10)
            .IsRequired(); // Ensure Type is required and has a max length

            entity.Property(t => t.Category)
            .HasMaxLength(50)
            .IsRequired(); // Ensure Category is required and has a max length

            // Define the relationship: user has many transactions
            entity.HasOne(t => t.AppUser)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.AppUserId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete transactions when user is deleted

        });
    }
}
