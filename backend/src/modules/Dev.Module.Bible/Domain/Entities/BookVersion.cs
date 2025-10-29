namespace Dev.Module.Bible.Domain.Entities;

public class BookVersion
{
    public Guid Id { get; set; }
    public Guid languageId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; }=string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
}
