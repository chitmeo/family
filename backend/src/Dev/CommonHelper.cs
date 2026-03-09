using Dev.Services;

namespace Dev;

public static class CommonHelper
{
    public static string GenerateRandomDigitCode(int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "length must be non-negative.");
        using var random = new SecureRandomNumberGenerator();
        var str = string.Empty;
        for (var i = 0; i < length; i++)
            str = string.Concat(str, random.Next(10).ToString());
        return str;
    }
    public static IDevFileProvider? DefaultFileProvider { get; set; }
}
