using Dev.Module.Auth.Application.UseCases.Roles.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.Role;

public partial class RoleController
{
    [HttpGet]
    public async Task<IActionResult> GetRolesAsync(CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetRolesQuery(), cancellationToken);
        return Ok(result);
    }
}
