namespace Dev.Module.Bible.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    public Guid VersionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
}
