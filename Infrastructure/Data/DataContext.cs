using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<User>()
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Role>()
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.Role)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
            .HasOne(m => m.Meeting)
            .WithMany(n => n.Notifications)
            .HasForeignKey(t => t.MeetingId);

        modelBuilder.Entity<Meeting>()
            .HasOne(u => u.User)
            .WithMany(m => m.Meetings)
            .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<Meeting>()
            .HasOne(u => u.User)
            .WithMany(m => m.Meetings)
            .HasForeignKey(u => u.UserId);

        modelBuilder.Entity<Notification>()
            .HasOne(u => u.User)
            .WithMany(n => n.Notifications)
            .HasForeignKey(u => u.UserId);

        base.OnModelCreating(modelBuilder);
    }
}