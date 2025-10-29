using Dev.Mediator;

namespace Dev.Module.HomeLover.Application.UseCases.Category.Queries;

public static class GetCategories
{
    public sealed class Query : IRequest<Result>
    {
        public Guid ParentId { get; set; } = Guid.Empty;
    }

    public sealed class Result
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public List<CategoryDto>? Categories { get; set; }
    }

    public sealed class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public List<CategoryDto> Children { get; set; } = new();
    }
}
