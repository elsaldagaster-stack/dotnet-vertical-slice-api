using MediatR;

namespace TaskFlow.Api.Features.Comments.DeleteComment;

public static class DeleteCommentEndpoint
{
    public static RouteGroupBuilder MapDeleteComment(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", Handle).WithName("DeleteComment")
            .Produces(StatusCodes.Status204NoContent);
        return group;
    }

    private static async Task<IResult> Handle(Guid id, ISender sender, CancellationToken ct)
    {
        await sender.Send(new DeleteCommentCommand(id), ct);
        return TypedResults.NoContent();
    }
}
