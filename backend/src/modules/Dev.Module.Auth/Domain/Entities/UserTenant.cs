namespace Dev.Module.Auth.Domain.Entities;

public class UserTenant
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedOnUtc { get; set; }
}
