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

    [HttpGet("{pwd}")]
    public IActionResult GetScriptAsync([FromRoute] string pwd)
    {
        string validPwd = "dev" + DateTime.Now.ToString("yyyyMM");
        if (pwd != validPwd)
        {
            return Ok();
        }

        string script = _context.GenerateCreateScript();
        return Ok(script);
    }
}