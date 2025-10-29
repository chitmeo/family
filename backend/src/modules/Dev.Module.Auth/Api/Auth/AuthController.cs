using Dev.Mediator;

namespace Dev.Module.Auth.Api.Auth;

public partial class AuthController : BaseController
{
    public AuthController(IMediator mediator) : base(mediator)
    {

    }
}
