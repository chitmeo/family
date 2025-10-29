using System.Text.Json.Serialization;

using Dev.Common;
using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Domain.Entities;
using Dev.Module.Auth.Domain.Services;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Auth.Application.UseCases.Auths.Commands;

public static class Login
{
    public sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IAuthDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IWebHelper _webHelper;

        public Handler(IAuthDbContext context, IWebHelper webHelper, IJwtService jwtService)
        {
            _context = context;
            _webHelper = webHelper;
            _jwtService = jwtService;
        }

        public async Task<Result> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(r => r.UserRoles).ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => u.Username.Trim().ToLower() == request.UserName.Trim().ToLower(), cancellationToken);

            if (user == null)
                return new Result { Success = false, ErrorMessage = "User does not exist" };

            user.LastActivityDateUtc = DateTime.UtcNow;
            user.LastIpAddress = _webHelper.GetCurrentIpAddress();
            await _context.SaveChangesAsync(cancellationToken);

            string accessToken = _jwtService.CreateToken(user);
            string refreshToken = _jwtService.GenerateRefreshToken();
            RefreshToken newRefreshToken = new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshToken,
                AccessToken = accessToken,
                ExpiryDate = _jwtService.GetRefreshTokenExpirationDate(),
                Invalidated = false
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new Result
            {
                Success = true,
                AccessToken= accessToken,
                RefreshToken = refreshToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = request.UserName,
                    Email = user.Email,
                    Roles = string.Join(",", user.UserRoles.Select(r => r.Role.SystemName))
                }
            };
        }

        
    }
    public sealed class Result
    {
        public bool Success { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ErrorMessage { get; set; }
        public UserDto User { get; set; } = new();
    }

    public sealed class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
    }

    public sealed record Command(
       [property: JsonPropertyName("userName")] string UserName,
       [property: JsonPropertyName("password")] string Password
     ) : IRequest<Result>;

    //public sealed class Validator : AbstractValidator<Command>
    //{
    //    private readonly IAuthDbContext _context;
    //    private readonly IPasswordHasher _passwordHasher;

    //    public Validator(IAuthDbContext context, IPasswordHasher passwordHasher)
    //    {
    //        _context = context;
    //        _passwordHasher = passwordHasher;

    //        RuleFor(x => x.UserName)
    //            .NotEmpty().WithMessage("User is required")
    //            .MaximumLength(255).WithMessage("User must not exceed 255 characters")
    //            .MustAsync(UserExistsAsync).WithMessage("User does not exist");

    //        RuleFor(x => x.Password)
    //            .NotEmpty().WithMessage("Password is required")
    //            .MaximumLength(255).WithMessage("Password must not exceed 255 characters")
    //            .MustAsync(PasswordIsValidAsync).WithMessage("Password is invalid");
    //    }

    //    private async Task<bool> UserExistsAsync(string user, CancellationToken cancellationToken)
    //    {
    //        return await _context.Users
    //            .AnyAsync(u => u.Username.Trim().ToLower() == user.Trim().ToLower(), cancellationToken);
    //    }

    //    private async Task<bool> PasswordIsValidAsync(Command command, string password, CancellationToken cancellationToken)
    //    {
    //        var user = await _context.Users
    //            .FirstOrDefaultAsync(u => u.Username.Trim().ToLower() == command.UserName.Trim().ToLower(), cancellationToken);

    //        if (user == null || string.IsNullOrEmpty(password))
    //            return false;

    //        var userPassword = await _context.UserPasswords
    //            .OrderByDescending(u => u.CreatedOnUtc)
    //            .FirstOrDefaultAsync(u => u.UserId == user.Id, cancellationToken);

    //        if (userPassword == null)
    //            return false;

    //        string savedPassword = _passwordHasher.HashPassword(
    //            command.UserName,
    //            command.Password,
    //            userPassword.PasswordSalt,
    //            userPassword.PasswordFormat);

    //        return userPassword.Password == savedPassword;
    //    }
    //}
}
