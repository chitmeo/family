using Dev.Module.Auth.Application.UseCases.Roles.Commands;

using Microsoft.AspNetCore.Mvc;

using System.Threading;

namespace Dev.Module.Auth.Api.Role;

public partial class RoleController
{
    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);
        return Ok(result);
    }
}
