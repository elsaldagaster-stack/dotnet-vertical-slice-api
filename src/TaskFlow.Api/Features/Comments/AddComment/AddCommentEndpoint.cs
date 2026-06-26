using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Features.Comments.AddComment;

public static class AddCommentEndpoint
{
    public static RouteGroupBuilder MapAddComment(this RouteGroupBuilder group)
    {
        group.MapPost("/", Handle).WithName("AddComment")
            .Produces<Guid>(StatusCodes.Status201Created);
        return group;
    }

    private static async Task<IResult> Handle(
        Guid issueId, [FromBody] AddCommentRequest body, ISender sender, CancellationToken ct)
    {
        var id = await sender.Send(new AddCommentCommand(issueId, body.Content, body.AuthorId), ct);
        return TypedResults.Created($"/api/comments/{id}", id);
    }

    private record AddCommentRequest(string Content, Guid AuthorId);
}
