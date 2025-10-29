using Dev.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Domain.Interfaces;

public interface ICoreDbContext
{
    DbSet<Setting> Settings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync();
    string GenerateCreateScript();
}
