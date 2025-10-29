using Dev.Module.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Auth.Application.Interfaces.Persistence;

public interface IAuthDbContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<UserPassword> UserPasswords { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync();
    string GenerateCreateScript();
}
