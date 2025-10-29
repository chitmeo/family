namespace Dev.Module.Auth.Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ExpiryDate { get; set; } = DateTime.UtcNow;
    public bool Invalidated { get; set; } = false;

    // Navigation property
    public User User { get; set; } = default!;
}