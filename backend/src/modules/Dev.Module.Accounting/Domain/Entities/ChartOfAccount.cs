namespace Dev.Module.Accounting.Domain.Entities;

public class ChartOfAccount
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }
    public ICollection<Account> Accounts { get; internal set; } = new List<Account>();
}
