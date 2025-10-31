using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;
using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.ChartOfAccounts.Commands;

public static class CreateChartOfAccount
{
    public sealed class Command : IRequest<Guid>
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
            await ValidateAndThrow(request);

            var chartOfAccount = new ChartOfAccount()
            {
                Id = Guid.NewGuid(),
            
                Code = request.Code,
                Name = request.Name,
                IsActive = request.IsActive,
                DisplayOrder = request.DisplayOrder
            };
            await _context.ChartOfAccounts.AddAsync(chartOfAccount, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return chartOfAccount.Id;
        }

        private async Task ValidateAndThrow(Command request)
        {
            bool isDuplicate = await _context.ChartOfAccounts
                .AnyAsync(x => x.Code.ToLower() == request.Code.ToLower());

            if (isDuplicate)
            {
                throw new InvalidOperationException($"Chart of Account with code '{request.Code}' already exists.");
            }
        }
    }
}
