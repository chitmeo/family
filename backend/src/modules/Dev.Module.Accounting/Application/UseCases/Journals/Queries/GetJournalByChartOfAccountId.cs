using Dev.Mediator;

namespace Dev.Module.Accounting.Application.UseCases.Journals.Queries;

public static class GetJournalByChartOfAccountId
{
    public record Result
    {
        public Guid Id { get; set; }
        public Guid ChartOfAccountId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Guid DefaultDebitAccountId { get; set; }
        public Guid DefaultCreditAccountId { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
    public record Query : IRequest<List<Result>>
    {
        public Guid ChartOfAccountId { get; set; }
    }
}
