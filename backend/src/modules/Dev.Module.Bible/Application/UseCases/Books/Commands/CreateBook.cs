using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;
using Dev.Module.Bible.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.Books.Commands;

public sealed class CreateBook
{
    public sealed record Command : IRequest<Guid>
    {
        [Required]
        public Guid BookVersionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Abbreviation { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
    }

    internal class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IBibleDbContext _context;

        public Handler(IBibleDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request, cancellationToken);
            var book = new Book()
            {
                BookVersionId = request.BookVersionId,
                Name = request.Name,
                Abbreviation = request.Abbreviation,
                DisplayOrder = request.DisplayOrder
            };

            await _context.Books.AddAsync(book, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return book.Id;
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var bookVersionExists = await _context.BookVersions
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.BookVersionId, cancellationToken);
            if (!bookVersionExists)
                throw new InvalidOperationException($"BookVersion with id '{request.BookVersionId}' does not exists.");

            var duplicateName = await _context.Books
                .AsNoTracking()
                .AllAsync(b => b.Name.Trim() == request.Name.Trim(), cancellationToken);
            if (duplicateName)
                throw new InvalidOperationException($"BookVersion code '{request.Name}' already exists in this BookVersion.");
        }
    }
}
