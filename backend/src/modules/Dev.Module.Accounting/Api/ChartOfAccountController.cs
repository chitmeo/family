using Dev.Mediator;
using Dev.Module.Accounting.Application.UseCases.Accounts.Queries;
using Dev.Module.Accounting.Application.UseCases.ChartOfAccounts.Commands;
using Dev.Module.Accounting.Application.UseCases.ChartOfAccounts.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

public partial class ChartOfAccountController : BaseController
{
    public ChartOfAccountController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(CreateChartOfAccount.Command command, CancellationToken cancellationToken)
    {
        Guid newId = await _mediator.SendAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new { id = newId });
    }


    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateChartOfAccount.Command command,
        CancellationToken cancellationToken)
    {
        if (command == null)
            return BadRequest("Invalid request body.");
        command.Id = id;
        await _mediator.SendAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchAsync(
        [FromQuery] SearchChartOfAccount.Query query,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.SendAsync(query, cancellationToken));
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
        query = query with { ChartOfAccountId = coaId };
        var items = await _mediator.SendAsync(query, cancellationToken);
        return Ok(items);
    }
}