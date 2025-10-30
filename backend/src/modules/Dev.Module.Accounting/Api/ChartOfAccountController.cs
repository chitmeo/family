using Dev.Mediator;
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

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchAsync(
        [FromQuery] SearchChartOfAccount.Query query,
        CancellationToken cancellationToken)
    {
        var items = await _mediator.SendAsync(query, cancellationToken);
        return Ok(items);
    }
}