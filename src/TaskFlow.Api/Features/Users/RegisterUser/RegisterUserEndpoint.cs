using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Features.Users.RegisterUser;

public static class RegisterUserEndpoint
{
    public static RouteGroupBuilder MapRegisterUser(this RouteGroupBuilder group)
    {
        group.MapPost("/", Handle)
            .WithName("RegisterUser")
            .WithSummary("Register a new user")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        return group;
    }

    private static async Task<IResult> Handle(
        [FromBody] RegisterUserCommand command,
        ISender sender,
        CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return TypedResults.Created($"/api/users/{id}", id);
    }
}
