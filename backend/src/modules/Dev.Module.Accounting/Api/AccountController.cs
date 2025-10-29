using Dev.Mediator;
using Dev.Module.Accounting.Application.UseCases.Accounts.Commands;
using Dev.Module.Accounting.Application.UseCases.Accounts.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

public class AccountController : BaseController
{
    public AccountController(IMediator mediator) : base(mediator) { }
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateAccount.Command command,
        CancellationToken cancellationToken)
    {
        Guid newId = await _mediator.SendAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new { id = newId });
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAccountByChartOfAccountId.Query query,
        CancellationToken cancellationToken)
    {
        var items = await _mediator.SendAsync(query, cancellationToken);
        return Ok(items);
    }
}
