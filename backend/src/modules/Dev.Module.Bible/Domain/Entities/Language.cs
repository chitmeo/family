﻿namespace Dev.Module.Bible.Domain.Entities;

public class Language
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } =0;
}
