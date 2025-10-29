using System.Text.Json.Serialization;

using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Application.Interfaces.Security;
using Dev.Module.Auth.Application.UseCases.Roles.Queries;
using Dev.Module.Auth.Domain.Entities;
using Dev.Module.Auth.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dev.Module.Auth.Application.UseCases.Auths.Commands;

public static class Register
{
    public sealed class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IAuthDbContext _context;
        private readonly IMediator _mediator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IAuthDbContext context,
            IMediator mediator,
            IPasswordHasher passwordHasher,
            ILogger<Handler> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _logger = logger;
            _logger.LogDebug("RegisterCommandHandler initialized.");
        }

        public async Task<Guid> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Handling RegisterCommand for {Username} and {Password}", request.UserName, request.Password);

            string saltKey = _passwordHasher.CreateSaltKey();
            string password = _passwordHasher.HashPassword(request.UserName, request.Password, saltKey);
            Guid userId = Guid.NewGuid();

            Guid userRoleId = await GetDefaultRoleIdAsync(cancellationToken);

            UserPassword userPassword = new()
            {
                UserId = userId,
                Password = password,
                PasswordSalt = saltKey,
                PasswordFormatId = (int)_passwordHasher.GetPasswordFormat(),
                CreatedOnUtc = DateTime.UtcNow,
                User = new User()
                {
                    Id = userId,
                    Username = request.UserName,
                    CreatedOnUtc = DateTime.UtcNow,
                    Active = true,
                    Deleted = false
                },
            };

            UserRole userRole = new()
            {
                UserId = userId,
                RoleId = userRoleId
            };

            await _context.UserPasswords.AddAsync(userPassword, cancellationToken);
            await _context.UserRoles.AddAsync(userRole, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return userId;
        }
        private async Task<Guid> GetDefaultRoleIdAsync(CancellationToken cancellationToken)
        {
            var role = await _mediator.SendAsync(new GetRoleByNameQuery { Name = Constraints.RoleUser }, cancellationToken);
            return role?.Id ?? Guid.Empty;
        }
    }

    public sealed record Command(
        [property: JsonPropertyName("userName")] string UserName,
        [property: JsonPropertyName("password")] string Password
    ) : IRequest<Guid>;

    //public sealed class Validator : AbstractValidator<Command>
    //{
    //    private readonly IAuthDbContext _context;

    //    public Validator(IAuthDbContext context)
    //    {
    //        _context = context;

    //        RuleFor(x => x.UserName)
    //            .NotEmpty().WithMessage("User is required")
    //            .MaximumLength(255).WithMessage("User must not exceed 255 characters")
    //            .MustAsync(UserExistsAsync).WithMessage("User already exists");

    //        RuleFor(x => x.Password)
    //            .NotEmpty().WithMessage("Password is required")
    //            .MaximumLength(255).WithMessage("Password must not exceed 255 characters");
    //    }

    //    private async Task<bool> UserExistsAsync(string userName, CancellationToken cancellationToken)
    //    {
    //        return !await _context.Users
    //            .AnyAsync(u =>
    //                u.Username.Trim().ToLower() == userName.Trim().ToLower(),
    //                cancellationToken);
    //    }
    //}
}
