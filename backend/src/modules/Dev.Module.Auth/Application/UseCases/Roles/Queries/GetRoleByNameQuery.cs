using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Auth.Application.UseCases.Roles.Queries;

public class GetRoleByNameQuery : IRequest<RoleDto?>
{
    public required string Name { get; set; }
}

internal class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByNameQuery, RoleDto?>
{
    private readonly IAuthDbContext _context;

    public GetRoleByIdQueryHandler(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<RoleDto?> HandleAsync(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .Where(r => r.SystemName == request.Name)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                IsSystemRole = r.IsSystemRole,
                SystemName = r.SystemName,
                Active = r.Active
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
