using Dev.Module.HomeLover.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.HomeLover.Application.Persistence;
public interface IHomeLoverDbContext
{
    DbSet<Category> Categories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync();
    string GenerateCreateScript();
}
