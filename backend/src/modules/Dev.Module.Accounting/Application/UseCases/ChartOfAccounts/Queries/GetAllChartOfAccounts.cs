using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.ChartOfAccounts.Queries;

public static class GetAllChartOfAccounts
{
    public sealed record Query(bool ShowHidden = false) : IRequest<List<Result>>;
    public sealed record Result(
        Guid Id,
        string Code,
        string Name,
        bool IsActive
    );
    internal sealed class Handler : IRequestHandler<Query, List<Result>>
    {
        private readonly IAccountingDbContext _context;

        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<List<Result>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var query = _context.ChartOfAccounts
                .AsNoTracking();
            if (!request.ShowHidden)
            {
                query = query.Where(x => x.IsActive);
            }
            var items = await query
               .OrderBy(x => x.DisplayOrder)
               .Select(x => new Result(
                   x.Id,
                   x.Code,
                   x.Name,
                   x.IsActive
               ))
               .ToListAsync(cancellationToken);

            return items;
        }
    }
}