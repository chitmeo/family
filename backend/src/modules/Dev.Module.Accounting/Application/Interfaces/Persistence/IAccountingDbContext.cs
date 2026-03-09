using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.Interfaces.Persistence;

public interface IAccountingDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<ChartOfAccount> ChartOfAccounts { get; }
    DbSet<JournalBook> JournalBooks { get; }
    DbSet<JournalTemplate> JournalTemplates { get; }
    DbSet<JournalEntry> JournalEntries { get; }
    DbSet<JournalEntryLine> JournalEntryLines { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync();
    string GenerateCreateScript();
}
