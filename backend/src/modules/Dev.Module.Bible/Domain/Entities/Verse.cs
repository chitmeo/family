namespace Dev.Module.Bible.Domain.Entities;

public class Verse
{
    public Guid Id { get; set; }
    public Guid ChapterId { get; set; }
    public int VerseNumber { get; set; } = 0;
    public string VerseText { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsParagraphStart { get; set; } = false;
}
