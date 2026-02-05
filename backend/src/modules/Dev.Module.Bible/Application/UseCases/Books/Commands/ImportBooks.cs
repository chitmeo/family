using System.Text.Json;

using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;
using Dev.Module.Bible.Domain.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Bible.Application.UseCases.Books.Commands;

public static class ImportBooks
{
    public sealed class Command : IRequest<int>
    {
        public required IFormFile JsonFile { get; set; }
    }

    internal class Handler : IRequestHandler<Command, int>
    {
        private readonly IBibleDbContext _context;

        public Handler(IBibleDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(
            Command request,
            CancellationToken cancellationToken)
        {
            if (request.JsonFile == null || request.JsonFile.Length == 0)
                throw new InvalidOperationException("JSON file is empty.");

            if (!request.JsonFile.FileName.EndsWith(".json"))
                throw new InvalidOperationException("Invalid file format. JSON required.");

            using var stream = request.JsonFile.OpenReadStream();

            var importItems = await JsonSerializer.DeserializeAsync<
                List<ImportBookModel>>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                },
                cancellationToken);

            if (importItems == null || importItems.Count == 0)
                return 0;

            var books = new List<Book>();

            foreach (var item in importItems)
            {
                if (item.BookVersionId == Guid.Empty)
                    throw new InvalidOperationException("Book VersionId is required.");

                if (string.IsNullOrWhiteSpace(item.Name))
                    throw new InvalidOperationException("Book Name is required.");

                if (string.IsNullOrWhiteSpace(item.Abbreviation))
                    throw new InvalidOperationException("Abbreviation is required.");

                books.Add(new Book
                {
                    Id = Guid.NewGuid(),
                    BookVersionId = item.BookVersionId,
                    Name = item.Name.Trim(),
                    Abbreviation = item.Abbreviation.Trim(),
                    DisplayOrder = item.DisplayOrder
                });
            }
            var existingAbbreviations = await _context.Books
                .Where(x => books
                    .Select(b => b.Abbreviation)
                    .Contains(x.Abbreviation))
                .Select(x => x.Abbreviation)
                .ToListAsync(cancellationToken);

            if (existingAbbreviations.Any())
            {
                throw new InvalidOperationException(
                    $"Duplicate abbreviations found: {string.Join(", ", existingAbbreviations)}");
            }

            await _context.Books.AddRangeAsync(books, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return books.Count;
        }
    }
    // =========================
    // Internal import model
    // =========================
    private sealed class ImportBookModel
    {
        public Guid BookVersionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}
