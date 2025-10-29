using Dev.Mediator;
using Dev.Module.HomeLover.Application.Persistence;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.HomeLover.Api;

public class HomeController : BaseController
{
    private readonly IHomeLoverDbContext _context;
    public HomeController(IMediator mediator,
                          IHomeLoverDbContext context) : base(mediator)
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
