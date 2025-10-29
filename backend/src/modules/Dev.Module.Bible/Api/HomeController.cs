using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Bible.Api;

public class HomeController : BaseController
{
    private readonly IBibleDbContext _context;
    public HomeController(IMediator mediator,
                          IBibleDbContext context) : base(mediator)
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
