namespace Dev.Module.Auth.UseCases.Users.Queries;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }
}
