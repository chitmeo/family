using Dev.Module.Auth.Application.UseCases.Roles.Commands;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.Role;

public partial class RoleController
{
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoleAsync(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.SendAsync(new DeleteRoleCommand { Id = id }, cancellationToken);
        return Ok();
    }
}
