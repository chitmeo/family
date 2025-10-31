namespace Dev.Module.Accounting.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public Guid ChartOfAccountId { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; } = default!;
    public Guid? ParentId { get; set; }
    public Account? Parent { get; set; }
    public ICollection<Account> Children { get; set; } = new List<Account>();
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Asset, Liability, Equity, Income, Expense
    /// </summary>
    public string AccountType { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Journal> DebitJournals { get; internal set; } = new List<Journal>();
    public ICollection<Journal> CreditJournals { get; internal set; } = new List<Journal>();
}
