namespace Dev.Module.Auth.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSystemRole { get; set; } = false;
    public string SystemName { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public ICollection<UserRole>? UserRoles { get; internal set; }
}
