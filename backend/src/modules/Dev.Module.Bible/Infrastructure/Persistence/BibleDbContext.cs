using System.Reflection;

using Dev.Module.Bible.Application.Persistence;
using Dev.Module.Bible.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Infrastructure.Persistence;

internal class BibleDbContext : DbContext, IBibleDbContext
{
    public DbSet<Language> Languages => Set<Language>();

    public DbSet<BookVersion> BookVersions => Set<BookVersion>();

    public DbSet<Book> Books => Set<Book>();

    public DbSet<Chapter> Chapters => Set<Chapter>();

    public DbSet<Verse> Verses => Set<Verse>();

    public BibleDbContext(DbContextOptions<BibleDbContext> options) : base(options) { }

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
