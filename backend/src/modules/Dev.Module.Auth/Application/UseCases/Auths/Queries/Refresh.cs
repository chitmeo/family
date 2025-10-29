using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;
using Dev.Module.Auth.Domain.Services;

using Microsoft.EntityFrameworkCore;

namespace Dev.Module.Auth.Application.UseCases.Auths.Queries;

public static class Refresh
{

    public sealed class Handler : IRequestHandler<Query, Result>
    {
        private readonly IAuthDbContext _context;
        private readonly IJwtService _jwtService;
        public Handler(IAuthDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<Result> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var storedToken = await _context.RefreshTokens.Include(u => u.User).ThenInclude(ur => ur.UserRoles).ThenInclude(r => r.Role)
                                            .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken);
            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow || storedToken.Invalidated)
                throw new UnauthorizedAccessException("Invalid or expired refresh token.");

            string newAccessToken = _jwtService.CreateToken(storedToken.User);
            string refreshToken = _jwtService.GenerateRefreshToken();

            //update storedToken
            storedToken.AccessToken = newAccessToken;
            storedToken.Token = refreshToken;
            storedToken.ExpiryDate = _jwtService.GetRefreshTokenExpirationDate();
            await _context.SaveChangesAsync();
            return new Result(newAccessToken, refreshToken);
        }
    }


    public sealed record Query(
       string AccessToken,
       string RefreshToken
     ) : IRequest<Result>;

    public sealed record Result(string NewAccessToken, string RefreshToken);

    //public sealed class Validator : AbstractValidator<Query>
    //{
    //    private readonly IAuthDbContext _context;
    //    public Validator(IAuthDbContext context)
    //    {
    //        _context = context;
    //        RuleFor(x => x.RefreshToken).Must(BeValidRefreshToken).WithMessage("Invalid or expired refresh token.");
    //    }

    //    private bool BeValidRefreshToken(string token)
    //    {
    //        var storedToken = _context.RefreshTokens
    //                                  .FirstOrDefault(rt => rt.Token == token);
    //        if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow || storedToken.Invalidated)
    //        {
    //            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
    //        }
    //        return true;
    //    }
    //}
}
