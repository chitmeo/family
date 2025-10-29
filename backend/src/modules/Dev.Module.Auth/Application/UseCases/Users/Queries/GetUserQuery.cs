using System.Text.Json.Serialization;

using Dev.Mediator;
using Dev.Module.Auth.Application.Interfaces.Persistence;

using Microsoft.EntityFrameworkCore;
namespace Dev.Module.Auth.UseCases.Users.Queries;

public class GetUserQuery : IRequest<List<UserDto>>
{
    [JsonPropertyName("searchKey")]
    public string SearchKey { get; set; } = string.Empty;
}

public class GetUserQueryHanlder : IRequestHandler<GetUserQuery, List<UserDto>>
{
    private readonly IAuthDbContext _context;

    public GetUserQueryHanlder(IAuthDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> HandleAsync(GetUserQuery request, CancellationToken cancellationToken)
    {
       return await _context.Users
            .Where(x => request.SearchKey == "" || 
                (x.Username.Contains(request.SearchKey) || x.Username.Contains(request.SearchKey))
            )
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Gender = u.Gender,
                Email = u.Email,
                DateOfBirth = u.DateOfBirth,
            })
            .ToListAsync(cancellationToken);
    }
}
