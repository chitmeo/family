namespace Dev.Module.Auth.Application.UseCases.Roles.Queries;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsSystemRole { get; set; } = false;
    public string SystemName { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
}
