using Dev.Mediator;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api;
[Route("auth/[controller]")]
[ApiController]
public abstract  class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
