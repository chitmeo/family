using Dev.Mediator;
using Dev.Module.Accounting.Application.UseCases.JournalEntries.Queries;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dev.Module.Accounting.Api;

public class JournalEntryController : BaseController
{
    protected JournalEntryController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("by-chartofaccount/{chartOfAccountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByChartOfAccountIdAsync(
        [FromRoute] Guid chartOfAccountId,
        [FromQuery] GetJournalEntryByChartOfAccountId.Query query,
        CancellationToken cancellationToken)
    {
        query = query with { ChartOfAccountId = chartOfAccountId };
        var journalEntries = await _mediator.SendAsync(query, cancellationToken);
        return Ok(journalEntries);
    }
}

