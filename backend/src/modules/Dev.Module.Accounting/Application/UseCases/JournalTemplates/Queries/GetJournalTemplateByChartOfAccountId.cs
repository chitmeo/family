using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.JournalTemplates.Queries;

public static class GetJournalTemplateByChartOfAccountId
{
    public record Result
    {
        public Guid Id { get; set; }
        public Guid ChartOfAccountId { get; set; }
        public string COAName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Guid DefaultDebitAccountId { get; set; }
        public string DebitAccountCode { get; set; } = string.Empty;
        public Guid DefaultCreditAccountId { get; set; }
        public string CreditAccountCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
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
            var query = _context.JournalTemplates
                                .Include(x => x.ChartOfAccount)
                                .Include(x => x.DefaultDebitAccount)
                                .Include(x => x.DefaultCreditAccount)
                                .AsNoTracking()
                                .Where(x => x.ChartOfAccountId == request.ChartOfAccountId);

            if (!request.ShowHidden)
            {
                query = query.Where(x => x.IsActive);
            }

            var items = await query
                .Select(x => new Result
                {
                    Id = x.Id,
                    ChartOfAccountId = x.ChartOfAccountId,
                    COAName = x.ChartOfAccount.Name,
                    Code = x.Code,
                    Name = x.Name,
                    Type = x.Type,
                    DefaultDebitAccountId = x.DefaultDebitAccountId,
                    DebitAccountCode = x.DefaultDebitAccount.Code,
                    DefaultCreditAccountId = x.DefaultCreditAccountId,
                    CreditAccountCode = x.DefaultCreditAccount.Code,
                    Description = x.Description,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
