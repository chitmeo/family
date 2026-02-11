using System.ComponentModel.DataAnnotations;

using Dev.Exceptions;
using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;
using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using Org.BouncyCastle.Ocsp;

namespace Dev.Module.Accounting.Application.UseCases.JournalEntries.Commands;

public class CreateJournalEntry
{
    public sealed record Command : IRequest<Guid>
    {
        /// <summary>
        /// Journal Template Id
        /// </summary>
        [Required]
        public Guid TemplateId { get; set; }
        [Required]
        public DateTime EntryDate { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Amount { get; set; } = 0;
        [Required]
        public JournalEntryStatus Status { get; set; }
    }
    internal class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IAccountingDbContext _context;
        private Guid journalBookId;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request, cancellationToken);
            var book = await _context.JournalBooks
                                .AsNoTracking()
                                .Where(x => x.IsActive)
                                .FirstOrDefaultAsync(cancellationToken);

            var template = await _context.JournalTemplates
                                .AsNoTracking()
                                .Where(x => x.Id == request.TemplateId)
                                .FirstOrDefaultAsync(cancellationToken);
            if (book == null || template == null)
                return Guid.Empty;

            var entry = new JournalEntry()
            {
                Id = Guid.NewGuid(),
                JournalBookId = book.Id,
                JournalTemplateId = template.Id,
                EntryDate = request.EntryDate,
                Reference = request.Reference,
                TotalAmount = request.Amount,
                Status = (int)request.Status
            };
            entry.JournalEntryLines.Add(new JournalEntryLine()
            {
                JournalEntryId = entry.Id,
                AccountId = template.DefaultCreditAccountId,
                Amount = request.Amount
            });
            entry.JournalEntryLines.Add(new JournalEntryLine()
            {
                JournalEntryId = entry.Id,
                AccountId = template.DefaultDebitAccountId,
                Amount = request.Amount
            });

            await _context.JournalEntries.AddAsync(entry, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entry.Id;
        }
        /// <summary>
        /// must has active JournalBook
        /// must exist JournalTemplate
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            bool isTemplateExits = await _context.JournalTemplates
                           .AnyAsync(x => x.Id == request.TemplateId);

            if (!isTemplateExits)
            {
                throw new NotFoundException(nameof(JournalTemplate), request.TemplateId);
            }
        }
    }
}
