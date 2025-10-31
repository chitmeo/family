using Dev.Helpers;
using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Accounting.Application.UseCases.Accounts.Commands;

public static class UpdateAccount
{
    public sealed class Command : IRequest<int>
    {
        public Guid Id { get; set; }
        public Guid ChartOfAccountId { get; set; }
        public Guid? ParentId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    public sealed class Handler : IRequestHandler<Command, int>
    {
        private readonly IAccountingDbContext _context;
        public Handler(IAccountingDbContext context)
        {
            _context = context;
        }

        public async Task<int> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            ValidationHelper.ValidateAndThrow(request);

            var entity = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return 0;
            }

            entity.ParentId = request.ParentId;
            entity.ChartOfAccountId = request.ChartOfAccountId;
            entity.Code = request.Code;
            entity.Name = request.Name;
            entity.AccountType = request.AccountType;
            entity.IsActive = request.IsActive;

            _context.Accounts.Update(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
