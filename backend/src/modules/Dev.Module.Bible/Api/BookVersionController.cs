using Dev.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Dev.Module.Bible.Application.UseCases.BookVersions.Commands;
using Dev.Module.Bible.Application.UseCases.BookVersions.Queries;

namespace Dev.Module.Bible.Api;

public class BookVersionController : BaseController
{
    public BookVersionController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllBookVersion.Query();
            var bookVersions = await _mediator.SendAsync(query,cancellationToken);
            return Ok(bookVersions);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAsync([FromBody] CreateBookVersion.Command command, CancellationToken cancellationToken)
    {
        try
        {
            Guid newId = await _mediator.SendAsync(command, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutAsync(
        [FromBody] UpdateBookVersion.Command command,
        CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.SendAsync(command, cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
        }
    }
}

