using Dev.Exceptions;
using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Domain.Entities;

namespace Dev.Module.Auth.Application.UseCases.Roles.Commands;

public class DeleteRoleCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

//public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
//{
//    private readonly IAuthDbContext _context;
//    public DeleteRoleCommandValidator(IAuthDbContext context)
//    {
//        _context = context;
//        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
//        RuleFor(x => x.Id).Must((id) =>
//        {
//            var exists = _context.Roles.Any(c => c.Id == id);
//            return exists;
//        }).WithMessage("Role not found.");
//    }
//}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IAuthDbContext _context;
    public DeleteRoleCommandHandler(IAuthDbContext context)
    {
        _context = context;
    }
    public async Task<bool> HandleAsync(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FindAsync(request.Id);
        if (role == null)
        {
            throw new NotFoundException(nameof(Role), request.Id);
        }
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
