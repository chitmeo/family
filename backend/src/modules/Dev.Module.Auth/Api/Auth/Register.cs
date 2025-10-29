using Dev.Module.Auth.Application.UseCases.Auths.Commands;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.Auth;

public partial class AuthController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(Register.Command command, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.SendAsync(command, cancellationToken));
    }
}
