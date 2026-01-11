using Microsoft.EntityFrameworkCore;
using NotatnikAPI.Models;

namespace NotatnikAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        modelBuilder.Entity<User>()
            .Property(u => u.PasswordHash)
            .IsRequired();

        // Configure Note entity
        modelBuilder.Entity<Note>()
            .Property(n => n.Content)
            .IsRequired();

        // Configure relationship
        modelBuilder.Entity<Note>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notes)
            .HasForeignKey(n => n.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
