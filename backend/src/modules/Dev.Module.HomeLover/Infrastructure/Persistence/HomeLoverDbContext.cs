using System.Reflection;

using Dev.Module.HomeLover.Application.Persistence;
using Dev.Module.HomeLover.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.HomeLover.Infrastructure.Persistence;
internal class HomeLoverDbContext : DbContext, IHomeLoverDbContext
{
    public DbSet<Category> Categories => Set<Category>();

    public HomeLoverDbContext(DbContextOptions<HomeLoverDbContext> options) : base(options) { }
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
