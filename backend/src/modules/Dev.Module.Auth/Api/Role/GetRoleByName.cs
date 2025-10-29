using Dev.Module.Auth.Application.UseCases.Roles.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.Role;

public partial class RoleController
{
    [HttpGet("{name}")]
    public async Task<IActionResult> GetRoleByNameAsync(string name, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(new GetRoleByNameQuery { Name = name }, cancellationToken);
        return Ok(result);
    }
}
