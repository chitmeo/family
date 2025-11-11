using Dev.Mediator;
using Dev.Module.Accounting.Application.UseCases.JournalBooks.Commands;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

public class JournalBookController : BaseController
{
    protected JournalBookController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateJournalBook.Command command,
        CancellationToken cancellationToken)
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
        [FromBody] UpdateJournalBook.Command command,
        CancellationToken cancellationToken)
    {
        if (command == null)
            return BadRequest("Invalid request body.");
        command = command with { Id = id };       
        await _mediator.SendAsync(command, cancellationToken);
        return NoContent();
    }

}

