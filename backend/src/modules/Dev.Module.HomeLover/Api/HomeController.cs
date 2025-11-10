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
