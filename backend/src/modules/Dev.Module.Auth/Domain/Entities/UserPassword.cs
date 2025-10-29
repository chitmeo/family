using System.ComponentModel.DataAnnotations.Schema;

namespace Dev.Module.Auth.Domain.Entities;

public class UserPassword
{
    public Guid Id { get; set; }
    public UserPassword()
    {
        PasswordFormat = PasswordFormat.Clear;
    }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Password { get; set; } = string.Empty;
    public int PasswordFormatId { get; set; }
    public string PasswordSalt { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
    [NotMapped]
    public PasswordFormat PasswordFormat
    {
        get => (PasswordFormat)PasswordFormatId;
        set => PasswordFormatId = (int)value;
    }
}
