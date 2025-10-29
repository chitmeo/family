using Dev.Module.Auth.Application.UseCases.Auths.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.Auth;

public partial class AuthController
{
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(Refresh.Query query, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(query, cancellationToken);
        if (result == null)
            return Unauthorized();

        return Ok(result);
    }
}
