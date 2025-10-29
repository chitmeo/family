using Dev.Mediator;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

[Route("accg/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
