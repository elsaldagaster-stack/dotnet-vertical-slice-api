using MediatR;

namespace TaskFlow.Api.Features.Users.GetUsers;

public record GetUsersQuery : IRequest<IReadOnlyList<UserDto>>;

public record UserDto(Guid Id, string Name, string Email, DateTimeOffset CreatedAt);
