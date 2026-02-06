using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.BookVersions.Queries;

public sealed class GetAllBookVersion
{
    public sealed record Result(
        Guid Id,
        string Name,
        string Code,
        string Description,
        int DisplayOrder
    );

    public record Query : IRequest<List<Result>>;

    internal sealed class Handler : IRequestHandler<Query, List<Result>>
    {
        private readonly IBibleDbContext _context;
        public Handler(IBibleDbContext context)
        {
            _context = context;
        }

        public async Task<List<Result>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var bookVersions = await _context.BookVersions
                .AsNoTracking()
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new Result(
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Description,
                    x.DisplayOrder
                ))
                .ToListAsync(cancellationToken);
            return bookVersions;
        }
    }
}
