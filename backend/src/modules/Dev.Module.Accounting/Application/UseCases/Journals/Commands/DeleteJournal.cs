using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.Journals.Commands;

public static class DeleteJournal
{
    public record Command : IRequest<int>
    {
        public Guid Id { get; set; }
    }

    internal class Handler : IRequestHandler<Command, int>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.Journals.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return 0;
            }
            _context.Journals.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}