
using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;
using Dev.Module.Bible.Domain.Entities;

namespace Dev.Module.Bible.Application.UseCases.Languages.Commands;

public static class CreateLanguage
{
    public sealed class Command : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
    }

    internal class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IBibleDbContext _context;
        public Handler(IBibleDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> HandleAsync(Command request, CancellationToken cancellationToken) {
            var language = new Language()
            {
                Code = request.Code,
                Name = request.Name,
                DisplayOrder = request.DisplayOrder,
            };
            await _context.Languages.AddAsync(language, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return language.Id;
        }
    }
}
