namespace Dev.Module.Accounting.Domain.Entities;

public class JournalEntryLine
{
    public Guid Id { get; set; }
    public Guid JournalEntryId { get; set; }
    public JournalEntry JournalEntry { get; set; } = default!;    
    public Guid AccountId { get; set; }
    public decimal Debit { get; set; } = 0;
    public decimal Credit { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
}
