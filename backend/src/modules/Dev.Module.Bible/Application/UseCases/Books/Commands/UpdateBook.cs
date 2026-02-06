using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.Books.Commands;

public sealed class UpdateBook
{
    public record Command : IRequest<int>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid BookVersionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Abbreviation { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
    }

    internal class Handler : IRequestHandler<Command, int>
    {
        private readonly IBibleDbContext _context;

        public Handler(IBibleDbContext context)
        {
            _context = context;
        }
        public async Task<int> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request, cancellationToken);
            var book = await _context.Books.FindAsync(request.Id, cancellationToken);
            if (book == null)
                return 0;

            book.Name = request.Name;
            book.Abbreviation = request.Abbreviation;
            book.DisplayOrder = request.DisplayOrder;

            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var duplicateAbbreviation = await _context.Books
                            .AsNoTracking()
                            .AllAsync(x => x.Abbreviation.Trim() == request.Abbreviation.Trim() && x.Id != request.Id, cancellationToken);
            if (duplicateAbbreviation)
                throw new InvalidOperationException($"Book Abbreviation '{request.Abbreviation}' already exists in this Book.");

        }
    }


}
