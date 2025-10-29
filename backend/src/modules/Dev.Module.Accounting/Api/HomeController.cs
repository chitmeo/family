using Dev.Mediator;
using Dev.Module.Accounting.Application.Interfaces.Persistence;

using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

public class HomeController : BaseController
{
    private readonly IAccountingDbContext _context;
    public HomeController(IMediator mediator, IAccountingDbContext context) : base(mediator)
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