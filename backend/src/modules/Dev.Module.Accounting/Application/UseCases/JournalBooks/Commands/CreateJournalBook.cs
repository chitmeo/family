using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;
using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;

using Org.BouncyCastle.Ocsp;

namespace Dev.Module.Accounting.Application.UseCases.JournalBooks.Commands;

public static class CreateJournalBook
{
    public sealed record Command : IRequest<Guid>
    {
        [Required(ErrorMessage = "ChartOfAccountId is required.")]
        public Guid ChartOfAccountId { get; init; }

        [Required(ErrorMessage = "Code is required.")]
        [StringLength(10, ErrorMessage = "Code length must not exceed 10 characters.")]
        public string Code { get; init; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, ErrorMessage = "Name length must not exceed 255 characters.")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "PeriodStart is required.")]        
        public DateTime  PeriodStart { get; set; }

        [Required(ErrorMessage = "PeriodEnd is required.")]
        public DateTime  PeriodEnd { get; set; }

        [StringLength(1024, ErrorMessage = "Description length must not exceed 1024 characters.")]
        public string Description { get; init; } = string.Empty;
        public bool IsActive { get; init; } = false;
    }


    internal sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request, cancellationToken);
            var journalBook = new JournalBook()
            {
                ChartOfAccountId = request.ChartOfAccountId,
                Code = request.Code,
                Name = request.Name,
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd,
                Description = request.Description,
                IsActive = request.IsActive
            };
            await _context.JournalBooks.AddAsync(journalBook, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return journalBook.Id;
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var isDuplicate = await _context.JournalBooks
                .AsNoTracking()
                .AnyAsync(x => x.Code == request.Code, cancellationToken);

            if (isDuplicate)
                throw new InvalidOperationException($"Journal Book with code '{request.Code}' already exists.");
        }
    }
}

