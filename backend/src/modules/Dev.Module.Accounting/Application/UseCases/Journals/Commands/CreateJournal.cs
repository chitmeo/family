using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

namespace Dev.Module.Accounting.Application.UseCases.Journals.Commands;

public static class CreateJournal
{
    public sealed class Command : IRequest<Guid>
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
            var journal = new Domain.Entities.Journal()
            {
                Id = Guid.NewGuid(),
                ChartOfAccountId = request.ChartOfAccountId,
                Code = request.Code,
                Name = request.Name,
                Type = request.Type,
                DefaultDebitAccountId = request.DefaultDebitAccountId,
                DefaultCreditAccountId = request.DefaultCreditAccountId,
                Description = request.Description,
                IsActive = request.IsActive
            };
            await _context.Journals.AddAsync(journal);
            await _context.SaveChangesAsync(cancellationToken);
            return journal.Id;
        }
    }
}

