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

    public sealed record Query(Guid ChartOfAccountId, bool ShowHidden = false) : IRequest<List<Result>>;

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
            var items = await query
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

            return items;
        }
    }
}
