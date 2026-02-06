using System;
using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.Languages.Commands;

public sealed class UpdateLanguage
{
    public sealed record Command : IRequest<int>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
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

            var language = await _context.Languages.FindAsync(request.Id, cancellationToken);
            if (language == null)
                return 0;

            language.Code = request.Code;
            language.Name = request.Name;
            language.DisplayOrder = request.DisplayOrder;

            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var duplicateCode = await _context.Languages
                            .AsNoTracking()
                            .AllAsync(x => x.Code.Trim() == request.Code.Trim() && x.Id != request.Id, cancellationToken);
            if (duplicateCode)
                throw new InvalidOperationException($"Language code '{request.Code}' already exists in this Language.");

        }
    }
}
