namespace Dev.Module.Auth.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public string TimeZoneId { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    public bool Deleted { get; set; } = false;

    public string LastIpAddress { get; set; } = string.Empty;

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? LastLoginDateUtc { get; set; }

    public DateTime LastActivityDateUtc { get; set; }

    public ICollection<UserPassword> UserPasswords { get; set; } = new List<UserPassword>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
