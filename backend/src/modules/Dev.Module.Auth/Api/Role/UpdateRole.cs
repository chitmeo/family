using Dev.Module.Auth.Application.UseCases.Roles.Commands;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.Role;

public partial class RoleController
{

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoleAsync(Guid id, UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await _mediator.SendAsync(command, cancellationToken);
        return Ok(result);
    }
}
