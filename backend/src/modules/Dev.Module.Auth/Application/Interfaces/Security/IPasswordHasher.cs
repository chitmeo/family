
using Dev.Module.Auth.Domain.Entities;

namespace Dev.Module.Auth.Application.Interfaces.Security;

public interface IPasswordHasher
{
    string HashPassword(string userName, string password, string saltKey, PasswordFormat passwordFormat = PasswordFormat.Hashed);
    PasswordFormat GetPasswordFormat();

    string CreateSaltKey();
}
