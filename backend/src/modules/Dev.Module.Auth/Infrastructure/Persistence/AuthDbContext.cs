using System.Reflection;

using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Auth.Infrastructure.Persistence;

public class AuthDbContext : DbContext, IAuthDbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public DbSet<UserPassword> UserPasswords => Set<UserPassword>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public string GenerateCreateScript()
    {
        return base.Database.GenerateCreateScript();
    }

    public async Task SaveChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}
