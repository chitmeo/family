using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.Accounts.Queries;

public static class GetAccountByChartOfAccountId
{
    public sealed record Result(
        Guid Id,
        Guid? ParentId,
        string? ParentCode,
        string Code,
        string Name,
        string AccountType,
        bool IsActive
    );

    public sealed record Query : IRequest<List<Result>>
    {
        public Guid ChartOfAccountId { get; init; }
        public bool ShowHidden { get; init; } = false;
        public string ParentCode { get; init; } = string.Empty;
    }

    internal sealed class Handler : IRequestHandler<Query, List<Result>>
    {
        private readonly IAccountingDbContext _context;

        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Result>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Accounts.AsNoTracking();
            if (!request.ShowHidden)
            {
                query = query.Where(x => x.IsActive);
            }
            var allAccounts = await query
                .Where(x => x.ChartOfAccountId == request.ChartOfAccountId)
                .OrderBy(x => x.Code)
                .Select(x => new Result(
                    x.Id,
                    x.ParentId,
                    x.Parent != null ? x.Parent.Code : null,
                    x.Code,
                    x.Name,
                    x.AccountType,
                    x.IsActive
                ))
                .ToListAsync(cancellationToken);

            if (request.ParentCode != "")
            {
                var parent = allAccounts.FirstOrDefault(x => x.Code == request.ParentCode);
                if (parent != null)
                {
                    var dict = allAccounts.ToLookup(x => x.ParentId);
                    List<Result> GetChildrenRecursive(Guid parentId)
                    {
                        var children = dict[parentId].ToList();
                        var result = new List<Result>();
                        foreach (var child in children)
                        {
                            result.Add(child);
                            result.AddRange(GetChildrenRecursive(child.Id));
                        }
                        return result;
                    }
                    allAccounts = GetChildrenRecursive(parent.Id);
                    allAccounts.Insert(0, parent);
                }
                else
                {
                    allAccounts = new List<Result>();
                }
            }
            return allAccounts;
        }
    }
}
