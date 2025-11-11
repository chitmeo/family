using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.JournalTemplates.Commands;

public static class UpdateJournalTemplate
{
    public sealed record Command : IRequest<Guid>
    {
        public Guid Id { get; set; }

        [Required]
        public Guid ChartOfAccountId { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Code cannot exceed 20 characters.")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "Type cannot exceed 10 characters.")]
        public string Type { get; set; } = string.Empty;

        [Required]
        public Guid DefaultDebitAccountId { get; set; }

        [Required]
        public Guid DefaultCreditAccountId { get; set; }

        [MaxLength(255, ErrorMessage = "Description  cannot exceed 255 characters.")]
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    internal class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request);
            var journal = await _context.JournalTemplates.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
             if (journal == null)
            {
                throw new KeyNotFoundException($"Journal with ID {request.Id} not found.");
            }
            journal.ChartOfAccountId = request.ChartOfAccountId;
            journal.Code = request.Code;
            journal.Name = request.Name;
            journal.Type = request.Type;
            journal.DefaultDebitAccountId = request.DefaultDebitAccountId;
            journal.DefaultCreditAccountId = request.DefaultCreditAccountId;
            journal.Description = request.Description;
            journal.IsActive = request.IsActive;

            await _context.SaveChangesAsync(cancellationToken);
            return journal.Id;
        }

        private async Task ValidateAndThrow(Command request)
        {
            bool isDuplicate = await _context.JournalTemplates
                .AnyAsync(x => x.ChartOfAccountId == request.ChartOfAccountId &&
                               x.DefaultDebitAccountId == request.DefaultDebitAccountId &&
                               x.DefaultCreditAccountId == request.DefaultCreditAccountId &&
                               x.Id != request.Id, CancellationToken.None);

            if (isDuplicate)
            {
                throw new InvalidOperationException($"Journal already exists.");
            }
        }
    }
}
