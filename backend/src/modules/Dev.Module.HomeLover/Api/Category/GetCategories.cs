using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.HomeLover.Api.Category;

public partial class CategoryController
{
    [HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        //var query = new GetCategories.Query();
        //var result = await Sender.SendAsync(query, cancellationToken);
        //return Ok(result);
        return Ok();
    }
}
