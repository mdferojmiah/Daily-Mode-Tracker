using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Models.Entities;

namespace MoodTracker.Data;

public class AppDbContext : IdentityDbContext<User, UserRole, Guid>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<MoodEntry> MoodEntries { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}