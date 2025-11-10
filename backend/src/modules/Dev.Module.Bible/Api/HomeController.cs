using Dev.Mediator;
using Dev.Module.Bible.Application.Persistence;

using Microsoft.AspNetCore.Http;
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

    [HttpGet("create-admin{pwd}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateAdminAsync([FromRoute] string pwd)
    {
        string validPwd = "dev" + DateTime.Now.ToString("yyyyMM");
        if (pwd != validPwd)
        {
            return Ok();
        }

        return Ok();
    }
}
    
