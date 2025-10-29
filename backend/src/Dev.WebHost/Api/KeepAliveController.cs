using Microsoft.AspNetCore.Mvc;

namespace Dev.WebHost.Api;

[Route("host/[controller]")]
[ApiController]
public class KeepAliveController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok($"Alive at {DateTime.UtcNow:O}");
    }
}
