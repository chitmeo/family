using Dev.Mediator;
using Dev.Module.Accounting.Application.UseCases.Journals.Commands;
using Dev.Module.Accounting.Application.UseCases.Journals.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

public class JournalController : BaseController
{
    public JournalController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> CreateAsync([FromBody] CreateJournal.Command command, CancellationToken cancellationToken)
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
        [FromBody] UpdateJournal.Command command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("Mismatched Journal ID");
        }

        await _mediator.SendAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteJournal.Command { Id = id };
        await _mediator.SendAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("by-chartofaccount/{chartOfAccountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByChartOfAccountIdAsync(
        [FromRoute] Guid chartOfAccountId,
        [FromQuery] GetJournalByChartOfAccountId.Query query,
        CancellationToken cancellationToken)
    {
        query = query with { ChartOfAccountId = chartOfAccountId };
        var journal = await _mediator.SendAsync(query, cancellationToken);
        return Ok(journal);
    }
}