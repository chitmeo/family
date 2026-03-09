using Dev.Mediator;
using Dev.Module.Accounting.Application.UseCases.JournalEntries.Commands;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

public class JournalEntryController : BaseController
{
    public JournalEntryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [FromBody]CreateJournalEntry.Command command,
        CancellationToken cancellationToken)
    {
        Guid newId = await _mediator.SendAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new {id = newId});
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateStatusAsync(
        [FromBody] UpdateJournalEntryStatus.Command command,
        CancellationToken cancellationToken
    )
    {
        await _mediator.SendAsync(command, cancellationToken);
        return NoContent();
    }
}

