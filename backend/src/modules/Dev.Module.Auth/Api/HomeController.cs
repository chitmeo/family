using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Auth.Api;

public class HomeController : BaseController
{
    private readonly IAuthDbContext _context;

    public HomeController(
        IMediator mediator,
        IAuthDbContext context) 
        : base(mediator)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetScriptAsync()
    {
        string script = _context.GenerateCreateScript();
        return Ok(script);
    }
}
