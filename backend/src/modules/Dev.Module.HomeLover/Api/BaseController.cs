using Dev.Mediator;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.HomeLover.Api;

[Route("hlo/[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
