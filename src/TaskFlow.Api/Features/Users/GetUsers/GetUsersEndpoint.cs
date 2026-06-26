using MediatR;

namespace TaskFlow.Api.Features.Users.GetUsers;

public static class GetUsersEndpoint
{
    public static RouteGroupBuilder MapGetUsers(this RouteGroupBuilder group)
    {
        group.MapGet("/", Handle)
            .WithName("GetUsers")
            .WithSummary("Get all users")
            .Produces<IReadOnlyList<UserDto>>();
        return group;
    }

    private static async Task<IResult> Handle(ISender sender, CancellationToken ct)
        => TypedResults.Ok(await sender.Send(new GetUsersQuery(), ct));
}
