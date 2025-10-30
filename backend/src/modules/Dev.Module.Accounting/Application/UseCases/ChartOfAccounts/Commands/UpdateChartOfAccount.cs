using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

namespace Dev.Module.Accounting.Application.UseCases.ChartOfAccounts.Commands;

public static class UpdateChartOfAccount
{
    public sealed class Command : IRequest<bool>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [StringLength(10, ErrorMessage = "Code length must not exceed 10 characters.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(10, ErrorMessage = "Name length must not exceed 255 characters.")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 1;
    }

    internal class Handler : IRequestHandler<Command, bool>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }

        public Task<bool> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            throw new NotImplementedException();
        }
    }
}
