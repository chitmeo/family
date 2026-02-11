using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

namespace Dev.Module.Accounting.Application.UseCases.JournalBooks.Commands;

public static class CloseJournalBook
{
    public sealed record Command : IRequest<int>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, int>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }
        public async Task<int> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            await ValidateAndThrow(request, cancellationToken);
            var journalBook = await _context.JournalBooks.FindAsync(request.Id);
            if (journalBook == null)
                return 0;
            journalBook.IsActive = false;
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {

        }
    }
}
