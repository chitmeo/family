using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.JournalBooks.Queries;

public static class GetJournalBookByDateRange
{
    public sealed record Result
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public DateTime PeriodStart { get; init; }
        public DateTime PeriodEnd { get; init; }
        public bool IsActive { get; init; }
    }

    public sealed class Query : IRequest<IEnumerable<Result>>
    {
        [Required(ErrorMessage = "ChartOfAccountId is required.")]
        public Guid ChartOfAccountId { get; init; }
        [Required(ErrorMessage = "StartDate is required.")]
        public DateTime StartDate { get; init; }
        [Required(ErrorMessage = "EndDate is required.")]
        public DateTime EndDate { get; init; }
    }
    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Result>>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Result>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            ValidateAndThrow(request);

            return await _context.JournalBooks
                .AsNoTracking()
                .Where(x => x.PeriodStart <= request.EndDate && x.PeriodEnd >= request.StartDate)
                .OrderBy(x => x.PeriodStart)
                .Select(x => new Result
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    PeriodStart = x.PeriodStart,
                    PeriodEnd = x.PeriodEnd,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);
        }

        private void ValidateAndThrow(Query request)
        {
            if (request.EndDate < request.StartDate)
            {
                throw new InvalidOperationException("End date must be greater than or equal to start date.");
            }
        }
    }
}
