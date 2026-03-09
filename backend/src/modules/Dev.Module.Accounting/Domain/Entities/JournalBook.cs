namespace Dev.Module.Accounting.Domain.Entities;

public class JournalBook
{
    public Guid Id { get; set; }
    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; } = default!;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Only on Book is active
    /// </summary>
    public bool IsActive { get; set; } = false;
    public ICollection<JournalEntry> JournalEntries { get; set; } = new List<JournalEntry>();
}
