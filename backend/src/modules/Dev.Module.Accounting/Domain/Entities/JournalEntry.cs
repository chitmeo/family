namespace Dev.Module.Accounting.Domain.Entities;

public class JournalEntry
{
    public Guid Id { get; set; }
    public Guid JournalBookId { get; set; }
    public JournalBook JournalBook { get; set; } = default!;
    public Guid JournalTemplateId { get; set; }
    public JournalTemplate JournalTemplate { get; set; } = default!;    
    public DateTime EntryDate { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; } = 0;
    /// <summary>
    /// 0 draft, 1 posted, 2 cancelled
    /// </summary>
    public int Status { get; set; } = 1;
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}
