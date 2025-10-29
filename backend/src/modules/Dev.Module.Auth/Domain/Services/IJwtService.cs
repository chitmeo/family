using Dev.Module.Auth.Domain.Entities;

namespace Dev.Module.Auth.Domain.Services;
public interface IJwtService
{
    string CreateToken(User user);
    string GenerateRefreshToken();

    DateTime GetRefreshTokenExpirationDate();
}
