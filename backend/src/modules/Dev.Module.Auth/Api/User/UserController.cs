using Dev.Mediator;

namespace Dev.Module.Auth.Api.User;

public partial class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator)
    {
    }
}
