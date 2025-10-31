namespace Dev.Module.Accounting.Domain.Entities;

public class JournalEntry
{
    public Guid Id { get; set; }
    public Guid JournalId { get; set; }
    public Journal Journal { get; set; } = default!;    
    public DateTime EntryDate { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalDebit { get; set; } = 0;
    public decimal TotalCredit { get; set; } = 0;
    /// <summary>
    /// 0 draft, 1 posted, 2 cancelled
    /// </summary>
    public int Status { get; set; } = 1;
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}
