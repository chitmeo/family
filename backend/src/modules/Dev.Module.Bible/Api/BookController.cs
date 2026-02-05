using System.Text.Json;

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
        return Ok();
    }
}

