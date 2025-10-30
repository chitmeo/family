using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

namespace Dev.Module.Accounting.Application.UseCases.ChartOfAccounts.Commands;

public static class UpdateChartOfAccount
{
    public sealed class Command : IRequest<int>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Code is required.")]
        [MaxLength(10, ErrorMessage = "Code length must not exceed 10 characters.")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(255, ErrorMessage = "Name length must not exceed 255 characters.")]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 1;
    }

    internal class Handler : IRequestHandler<Command, int>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);
            await ValidateAndThrow(request);

            var entity = await _context.ChartOfAccounts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Chart of Account with ID {request.Id} not found.");
            }

            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.IsActive = request.IsActive;
            entity.DisplayOrder = request.DisplayOrder;
            return await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task ValidateAndThrow(Command request)
        {
            bool isDuplicate = await _context.ChartOfAccounts
                .AnyAsync(x => x.Code.ToLower() == request.Code.ToLower() && x.Id != request.Id);

            if (isDuplicate)
            {
                throw new InvalidOperationException($"Chart of Account with code '{request.Code}' already exists.");
            }
        }
    }
}
