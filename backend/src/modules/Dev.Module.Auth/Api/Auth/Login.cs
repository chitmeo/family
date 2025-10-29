using Dev.Module.Auth.Application.UseCases.Auths.Commands;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.Auth;

public partial class AuthController : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(Login.Command command, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(command, cancellationToken);
        if (!result.Success)
            return Unauthorized(new { message = result.ErrorMessage });

        return Ok(new { accessToken = result.AccessToken, 
                        refreshToken = result.RefreshToken, 
                        user = result.User });
    }
}
