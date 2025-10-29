using Dev.Mediator;

namespace Dev.Module.Auth.Api.Role;
public partial class RoleController : BaseController
{
    public RoleController(IMediator mediator) : base(mediator)
    {
    }
}
