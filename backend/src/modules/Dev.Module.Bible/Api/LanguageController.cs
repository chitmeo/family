using Dev.Mediator;
using Dev.Module.Bible.Application.UseCases.Languages.Commands;
using Dev.Module.Bible.Application.UseCases.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Bible.Api;

public class LanguageController : BaseController
{
    public LanguageController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        CreateLanguage.Command command,
        CancellationToken cancellationToken)
    {
        Guid newId = await _mediator.SendAsync(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, new { id = newId });
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var query = new GetAllLanguage.Query();
        var result =  await _mediator.SendAsync(query, cancellationToken);
        return Ok(result);
    }
}
