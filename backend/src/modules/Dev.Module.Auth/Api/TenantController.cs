using Dev.Mediator;

namespace Dev.Module.Auth.Api;

public class TenantController : BaseController
{
    public TenantController(IMediator mediator) : base(mediator)
    {
    }
}

