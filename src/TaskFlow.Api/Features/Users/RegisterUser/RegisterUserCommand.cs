using MediatR;

namespace TaskFlow.Api.Features.Users.RegisterUser;

public record RegisterUserCommand(string Name, string Email) : IRequest<Guid>;
