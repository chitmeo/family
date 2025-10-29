using System.ComponentModel.DataAnnotations;

using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;
using Dev.Module.Accounting.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.Accounts.Commands;

public static class CreateAccount
{
    public sealed class Command : IRequest<Guid>
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ChartOfAccountId { get; set; }        
        public Guid? ParentId { get; set; } 
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string AccountType { get; set; } = string.Empty;
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
            await ValidateAndThrow(request, cancellationToken);

            var account = new Account()
            {
                Id = Guid.NewGuid(),
                ChartOfAccountId = request.ChartOfAccountId,
                ParentId = request.ParentId,
                Code = request.Code,
                Name = request.Name,
                AccountType = request.AccountType,
                IsActive = request.IsActive
            };
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync(cancellationToken);
            return account.Id;
        }

        private async Task ValidateAndThrow(Command request, CancellationToken cancellationToken)
        {
            var chartExists = await _context.ChartOfAccounts
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.ChartOfAccountId, cancellationToken);

            if (!chartExists)
                throw new InvalidOperationException($"Chart of Account with id '{request.ChartOfAccountId}' does not exist.");


            var duplicateCode = await _context.Accounts
                .AsNoTracking()
                .AnyAsync(x => x.ChartOfAccountId == request.ChartOfAccountId && x.Code == request.Code, cancellationToken);

            if (duplicateCode)
                throw new InvalidOperationException($"Account code '{request.Code}' already exists in this Chart of Account.");
        }
    }
}