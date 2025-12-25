namespace Dev.Module.Auth.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PrimaryDomain { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public DateTime CreatedOnUtc { get; set; }
}
