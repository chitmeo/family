using System.Dynamic;
using System.Reflection;

using Dev.Module.Accounting.Application.Interfaces.Persistence;
using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Infrastructure.Persistence;

public class AccountingDbContext : DbContext, IAccountingDbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<ChartOfAccount> ChartOfAccounts => Set<ChartOfAccount>();
    public DbSet<JournalBook> JournalBooks => Set<JournalBook>();
    public DbSet<JournalTemplate> JournalTemplates => Set<JournalTemplate>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();

    public AccountingDbContext(DbContextOptions<AccountingDbContext> options) : base(options) { }
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
