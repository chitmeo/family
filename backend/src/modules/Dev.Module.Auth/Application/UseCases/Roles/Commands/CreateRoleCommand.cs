using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Domain.Entities;

using Microsoft.EntityFrameworkCore;


namespace Dev.Module.Auth.Application.UseCases.Roles.Commands;

public record CreateRoleCommand : IRequest<Guid>
{
    public string Name { get; set; } = string.Empty;
    public bool IsSystemRole { get; set; } = false;
    public string SystemName { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}


// public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
// {
//     private readonly IAuthDbContext _context;
//     public CreateRoleCommandValidator(IAuthDbContext context)
//     {
//         _context = context;
//         RuleFor(x => x.Name)
//             .NotEmpty().WithMessage("Name is required")
//             .MaximumLength(50).WithMessage("Name must not exceed 50 characters");
//         RuleFor(x => x.Name).Must((name) =>
//         {
//             var exists = _context.Roles.Any(c => c.Name.ToLower() == name.ToLower());
//             return !exists;
//         }).WithMessage("Name must be unique.");
//     }
// }


internal class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IAuthDbContext _context;
    public CreateRoleCommandHandler(IAuthDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> HandleAsync(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        await ValidateAndThrow(request);

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsSystemRole = request.IsSystemRole,
            SystemName = request.SystemName,
            Active = request.Active
        };
        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return role.Id;
    }

    private async Task ValidateAndThrow(CreateRoleCommand request)
    {
        var isDuplicate = await _context.Roles.AnyAsync(c => c.Name.ToLower() == request.Name.ToLower());
        if (isDuplicate)
            {
                throw new InvalidOperationException($"User Role with name '{request.Name}' already exists.");
            }
    }
}