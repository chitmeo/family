using Dev.Module.Auth.Application.Interfaces.Security;
using Dev.Module.Auth.Domain.Entities;
using Dev.Services;

namespace Dev.Module.Auth.Infrastructure.Services.Security;

public class PasswordHasherService : IPasswordHasher
{
    private readonly IEncryptionService _encryptionService;
    private PasswordFormat _passwordFormat;
    private readonly string _defaultHashedPasswordFormat;
    private readonly int _saltKeySize = 8;
    public PasswordHasherService(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
        _passwordFormat = PasswordFormat.Hashed;
        //https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.cryptoconfig?view=net-5.0#remarks
        _defaultHashedPasswordFormat = "SHA512";
    }

    public PasswordFormat GetPasswordFormat()
    {
        return _passwordFormat;
    }

    public string CreateSaltKey()
    {
        return _encryptionService.CreateSaltKey(_saltKeySize);
    }


    public string HashPassword(string userName, string password, string saltKey, PasswordFormat passwordFormat = PasswordFormat.Hashed)
    {
        string savedPassword = string.Empty;
        _passwordFormat = passwordFormat;
        switch (_passwordFormat)
        {
            case PasswordFormat.Clear:
                savedPassword = password;
                break;
            case PasswordFormat.Hashed:

                savedPassword = _encryptionService.CreatePasswordHash(password, saltKey, _defaultHashedPasswordFormat);
                break;
            case PasswordFormat.Encrypted:
                savedPassword = _encryptionService.EncryptText(password, userName);
                break;
        }

        return savedPassword;
    }


}
