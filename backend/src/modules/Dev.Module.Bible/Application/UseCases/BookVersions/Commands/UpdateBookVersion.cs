using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.BookVersions.Commands;

public sealed class UpdateBookVersion
{
    public sealed record Command : IRequest<int>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid LanguageId { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
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

            var bookVersion = await _context.BookVersions.FindAsync(request.Id, cancellationToken);
            if (bookVersion == null)
                return 0;

            bookVersion.Code = request.Code;
            bookVersion.Name = request.Name;
            bookVersion.Description = request.Description;
            bookVersion.DisplayOrder = request.DisplayOrder;

            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var languageExists = await _context.Languages
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.LanguageId, cancellationToken);
            if (!languageExists)
                throw new InvalidOperationException($"Language with id '{request.LanguageId}' does not exists.");

            var bookVersionExists = await _context.BookVersions
                .AsNoTracking()
                .AnyAsync(b => b.Id == request.Id, cancellationToken);

            if (!bookVersionExists)
                throw new InvalidOperationException($"BookVersion with id '{request.LanguageId}' does not exists.");

            var duplicateCode = await _context.BookVersions
                .AsNoTracking()
                .AllAsync(b => b.Code.Trim() == request.Code.Trim() && b.Id != request.Id, cancellationToken);
            if (duplicateCode)
                throw new InvalidOperationException($"BookVersion code '{request.Code}' already exists in this BookVersion.");
        }
    }
}
