using Dev.Mediator;
using Dev.Module.Bible.Application.UseCases.Books.Commands;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Bible.Api;

public class BookController : BaseController
{
    public BookController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportAsync(
        ImportBooks.Command command,
        CancellationToken cancellationToken)
    {
        int importedRowCount = await _mediator.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAsync(
        [FromBody] CreateBook.Command command,
        CancellationToken cancellationToken)
    {
        try
        {
            Guid newId = await _mediator.SendAsync(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, new { id = newId });
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
    public async Task<IActionResult> PustAsync([FromBody] UpdateBook.Command command, CancellationToken cancellationToken)
    {
        try
        {
            int updatedRowCound = await _mediator.SendAsync(command, cancellationToken);
            return StatusCode(StatusCodes.Status200OK);
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
}

