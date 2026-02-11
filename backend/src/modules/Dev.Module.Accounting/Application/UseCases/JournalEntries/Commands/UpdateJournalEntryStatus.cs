using System.ComponentModel.DataAnnotations;

using Dev.Exceptions;
using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;
using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.JournalEntries.Commands;

public static class UpdateJournalEntryStatus
{
    public sealed record Command : IRequest<int>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public JournalEntryStatus Status { get; set; }
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
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request, cancellationToken);

            var entry = await _context.JournalEntries.FindAsync([request.Id], cancellationToken);
            if (entry == null)
                return 0;

            entry.Status = (int)request.Status;
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            bool isExits = await _context.JournalEntries
                           .AnyAsync(x => x.Id == request.Id);

            if (!isExits)
            {
                throw new NotFoundException(nameof(JournalEntry), request.Id);
            }
        }
    }
}
