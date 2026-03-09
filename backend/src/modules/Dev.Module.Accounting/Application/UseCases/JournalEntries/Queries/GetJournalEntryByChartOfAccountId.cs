using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

namespace Dev.Module.Accounting.Application.UseCases.JournalEntries.Queries;

public static class GetJournalEntryByChartOfAccountId
{
    public record Result
    {
        public Guid Id { get; set; }
    }

    public record Query : IRequest<List<Result>>
    {
        public Guid ChartOfAccountId { get; set; }
        public bool ShowHidden { get; set; } = false;
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
            return null;
        }
    }

}
