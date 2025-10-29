using Dev.Module.Bible.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.Persistence;

public interface IBibleDbContext
{
    DbSet<Language> Languages { get; }
    DbSet<BookVersion> BookVersions { get; }
    DbSet<Book> Books { get; }
    DbSet<Chapter> Chapters { get; }
    DbSet<Verse> Verses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync();
    string GenerateCreateScript();
}
