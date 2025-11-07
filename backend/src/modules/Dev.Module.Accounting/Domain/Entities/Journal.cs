namespace Dev.Module.Accounting.Domain.Entities;

public class Journal
{
    public Guid Id { get; set; }
    /// <summary>
    /// require
    /// </summary>
    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; } = default!;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// cash, bank, sale
    /// </summary>
    public string Type { get; set; } = string.Empty;
    public Guid DefaultDebitAccountId { get; set; }
    public Account DefaultDebitAccount { get; set; } = default!;
    public Guid DefaultCreditAccountId { get; set; }
    public Account DefaultCreditAccount { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
