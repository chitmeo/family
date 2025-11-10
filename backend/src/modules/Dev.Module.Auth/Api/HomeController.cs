using System.Threading.Tasks;

using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Application.UseCases.Auths.Commands;
using Dev.Module.Auth.Application.UseCases.Roles.Commands;
using Dev.Module.Auth.Infrastructure.Persistence;

using Microsoft.AspNetCore.Http;
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

    [HttpGet("{pwd}")]
    public async Task<IActionResult> GetScriptAsync([FromRoute] string pwd)
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
    public async Task<IActionResult> CreateAdminAsync([FromRoute] string pwd)
    {
        string validPwd = "dev" + DateTime.Now.ToString("yyyyMM");
        if (pwd != validPwd)
        {
            return Ok();
        }

        //create user        
        Register.Command command = new Register.Command
        (
            "kyle@chitmeo.com", "strongpass@123"
        );
        Guid userId = await _mediator.SendAsync(command);
        return StatusCode(StatusCodes.Status201Created, new { id = userId });
    }

    [HttpGet("create-roles{pwd}")]
    public async Task<IActionResult> CreateRolesAsync([FromRoute] string pwd)
    {
        string validPwd = "dev" + DateTime.Now.ToString("yyyyMM");
        if (pwd != validPwd)
        {
            return Ok();
        }

        //create roles
        CreateRoleCommand createAdminRoleCommand = new CreateRoleCommand
        {
            Name = Constraints.RoleAdmin,
            IsSystemRole = true,
            SystemName = Constraints.RoleAdmin,
            Active = true
        };
        await _mediator.SendAsync(createAdminRoleCommand);
        CreateRoleCommand createUserRoleCommand = new CreateRoleCommand
        {
            Name = Constraints.RoleUser,
            IsSystemRole = true,
            SystemName = Constraints.RoleUser,
            Active = true
        };
        await _mediator.SendAsync(createUserRoleCommand);
        return Ok();
    }
 
}
