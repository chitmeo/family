using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.Queries;

public static class GetAllLanguage
{
    public sealed record Result(
        Guid Id,
        string Name,
        string Code,
        int DisplayOrder
    );
    public record Query : IRequest<List<Result>>    {

    }
    internal sealed class Handler : IRequestHandler<Query, List<Result>>
    {
        private readonly IBibleDbContext _context;
        public Handler(IBibleDbContext context)
        {
            _context = context;
        }
        public async Task<List<Result>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var languages = await _context.Languages
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new Result(
                    x.Id,
                    x.Name,
                    x.Code,
                    x.DisplayOrder
                ))
                .ToListAsync(cancellationToken);
            return languages;
        }
    }
}
