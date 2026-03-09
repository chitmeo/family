using System;

using Dev.Mediator;

using MySqlX.XDevAPI.Common;

namespace Dev.Module.Accounting.Application.UseCases.JournalBooks.Queries;

public static class GetLastActiveJournalBook
{
    public sealed record Query : IRequest<List<Result>>;
}
