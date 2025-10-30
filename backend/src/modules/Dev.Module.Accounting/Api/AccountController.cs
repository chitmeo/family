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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateAccount.Command command,
        CancellationToken cancellationToken)
    {
        Guid newId = await _mediator.SendAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new { id = newId });
    }

    [HttpGet("{coaid:guid}/accounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByChartOfAccountIdAsync(
        [FromRoute] Guid coaId,        
        [FromQuery] GetAccountByChartOfAccountId.Query query,
        CancellationToken cancellationToken)
    {
        query.ChartOfAccountId = coaId;
        var items = await _mediator.SendAsync(query, cancellationToken);
        return Ok(items);
    }
}
