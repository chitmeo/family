using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Auth.Application.UseCases.Roles.Queries;

public record GetRolesQuery : IRequest<List<RoleDto>>
{

}

internal class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<RoleDto>>
{
    private readonly IAuthDbContext _context;

    public GetRolesQueryHandler(IAuthDbContext context)
    {
        _context = context;
    }
    public async Task<List<RoleDto>> HandleAsync(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .OrderBy(r => r.Name)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                IsSystemRole = r.IsSystemRole,
                SystemName = r.SystemName,
                Active = r.Active
            })
            .ToListAsync(cancellationToken);
    }
}
