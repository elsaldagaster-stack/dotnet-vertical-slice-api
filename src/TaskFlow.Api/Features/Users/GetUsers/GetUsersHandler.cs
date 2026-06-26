using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Users.GetUsers;

public sealed class GetUsersHandler(AppDbContext db) : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
{
    public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        => await db.Users
            .OrderBy(u => u.Name)
            .Select(u => new UserDto(u.Id, u.Name, u.Email, u.CreatedAt))
            .ToListAsync(cancellationToken);
}
