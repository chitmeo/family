using Dev.Module.Auth.UseCases.Users.Queries;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api.User;

public partial class UserController
{
    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser([FromQuery] GetUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.SendAsync(request, cancellationToken);
        return Ok(result);
    }
}
