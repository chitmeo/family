using System.ComponentModel.DataAnnotations;

using Dev.Mediator;

namespace Dev.Module.Accounting.Application.UseCases.Journals.Commands;

public static class UpdateJournal
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
        public Handler()
        {
        }
        public async Task<Guid> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return request.Id;
        }
    }
}
