using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;
using Dev.Module.Bible.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.BookVersions.Commands;

public sealed class CreateBookVersion
{
    public sealed record Command : IRequest<Guid>
    {
        [Required]
        public Guid LanguageId { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
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

            var bookVersion = new BookVersion()
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
                DisplayOrder = request.DisplayOrder
            };
            await _context.BookVersions.AddAsync(bookVersion, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return bookVersion.Id;
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var languageExists = await _context.Languages
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.LanguageId, cancellationToken);
            if (!languageExists)
                throw new InvalidOperationException($"Language with id '{request.LanguageId}' does not exists.");

            var duplicateCode = await _context.BookVersions
                .AsNoTracking()
                .AllAsync(b => b.Code == request.Code, cancellationToken);
            if (duplicateCode)
                throw new InvalidOperationException($"BookVersion code '{request.Code}' already exists in this BookVersion.");
        }
    }
}
