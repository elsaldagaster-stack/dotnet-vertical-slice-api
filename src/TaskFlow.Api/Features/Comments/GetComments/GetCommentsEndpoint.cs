using MediatR;

namespace TaskFlow.Api.Features.Comments.GetComments;

public static class GetCommentsEndpoint
{
    public static RouteGroupBuilder MapGetComments(this RouteGroupBuilder group)
    {
        group.MapGet("/", Handle).WithName("GetComments");
        return group;
    }

    private static async Task<IResult> Handle(
        Guid issueId, ISender sender, string? cursor, int limit = 20, CancellationToken ct = default)
        => TypedResults.Ok(await sender.Send(new GetCommentsQuery(issueId, cursor, limit), ct));
}
