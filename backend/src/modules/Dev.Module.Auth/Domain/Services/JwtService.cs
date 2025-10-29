using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Dev.Module.Auth.Domain.Entities;
using Dev.Module.Auth.Infrastructure.Identity;

using Microsoft.IdentityModel.Tokens;

namespace Dev.Module.Auth.Domain.Services;
public class JwtService : IJwtService
{
    private readonly JwtConfig _jwtConfig;

    public JwtService(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public string CreateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiryMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        tokenDescriptor.Subject.AddClaims(user.UserRoles.Select(r => new Claim(ClaimTypes.Role, r.Role.SystemName)));

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public DateTime GetRefreshTokenExpirationDate()
    {
        DateTime expiryDate = DateTime.UtcNow;
        expiryDate = expiryDate.AddDays(_jwtConfig.RefreshTokenExpiryDays);
        return expiryDate;
    }
}
