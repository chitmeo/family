using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.JournalBooks.Commands;

public static class UpdateJournalBook
{
    public sealed record Command : IRequest<int>
    {
        [Required(ErrorMessage = "Id is required.")]
        public Guid Id { get; init; }
        [Required(ErrorMessage = "Code is required.")]
        [StringLength(10, ErrorMessage = "Code length must not exceed 10 characters.")]
        public string Code { get; init; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, ErrorMessage = "Name length must not exceed 255 characters.")]
        public string Name { get; init; } = string.Empty;

        [StringLength(1024, ErrorMessage = "Description length must not exceed 1024 characters.")]
        public string Description { get; init; } = string.Empty;
        public bool IsActive { get; init; } = false;
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
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request, cancellationToken);
            
            var journalBook = await _context.JournalBooks.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (journalBook == null)
            {
                throw new KeyNotFoundException($"Journal Book with ID {request.Id} not found.");
            }

            journalBook.Code = request.Code;
            journalBook.Name = request.Name;
            journalBook.Description = request.Description;
            journalBook.IsActive = request.IsActive;
            //TODO: make sure alway has only one Joural Book is active.
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var isDuplicate = await _context.JournalBooks
                .AsNoTracking()
                .AnyAsync(x => x.Code == request.Code && x.Id != request.Id, cancellationToken);

            if (isDuplicate)
                throw new InvalidOperationException($"Journal Book with code '{request.Code}' already exists.");
        }
    }
}


