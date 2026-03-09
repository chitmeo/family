using Dev.Exceptions;
using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Domain.Entities;

namespace Dev.Module.Auth.Application.UseCases.Roles.Commands;

public record UpdateRoleCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSystemRole { get; set; } = false;
    public string SystemName { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}

//public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
//{
//    private readonly IAuthDbContext _context;
//    public UpdateRoleCommandValidator(IAuthDbContext context)
//    {
//        _context = context;
//        RuleFor(x => x.Name)
//            .NotEmpty().WithMessage("Name is required")
//            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
//        RuleFor(x => x.Name).Must((name) =>
//        {
//            var exists = _context.Roles.Any(c => c.Name.ToLower() == name.ToLower());
//            return !exists;
//        }).WithMessage("Name must be unique.");
//    }
//}

internal class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Guid>
{
    private readonly IAuthDbContext _context;
    public UpdateRoleCommandHandler(IAuthDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> HandleAsync(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FindAsync(request.Id);
        if (role == null)
        {
            throw new NotFoundException(nameof(Role), request.Id);
        }
        role.Name = request.Name;
        role.IsSystemRole = request.IsSystemRole;
        role.SystemName = request.SystemName;
        role.Active = request.Active;
        await _context.SaveChangesAsync(cancellationToken);
        return role.Id;
    }
}
