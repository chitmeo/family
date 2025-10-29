namespace Dev.Module.Auth.Infrastructure.Identity;

public class JwtConfig
{
    public required string Key { get; set; } = Guid.NewGuid().ToString();
    public required string Issuer { get; set; } = string.Empty;
    public required string Audience { get; set; } = string.Empty;
    public int AccessTokenExpiryMinutes { get; set; } = 60;
    public int RefreshTokenExpiryDays { get; set; } = 7;
}
