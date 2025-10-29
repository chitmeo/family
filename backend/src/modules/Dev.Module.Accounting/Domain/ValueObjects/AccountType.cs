using Dev.SharedKernel;

namespace Dev.Module.Accounting.Domain.ValueObjects;

public sealed class AccountType : ValueObject
{
    public static readonly AccountType Asset = new(nameof(Asset));
    public static readonly AccountType Liability = new(nameof(Liability));
    public static readonly AccountType Equity = new(nameof(Equity));
    public static readonly AccountType Income = new(nameof(Income));
    public static readonly AccountType Expense = new(nameof(Expense));

    public string Name { get; }

    private AccountType(string name) => Name = name;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }

    public bool IsDebitType() => Name is nameof(Asset) or nameof(Expense);
}
