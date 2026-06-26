using MediatR;

namespace TaskFlow.Api.Features.Issues.DeleteIssue;

public static class DeleteIssueEndpoint
{
    public static RouteGroupBuilder MapDeleteIssue(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", Handle).WithName("DeleteIssue")
            .Produces(StatusCodes.Status204NoContent);
        return group;
    }

    private static async Task<IResult> Handle(Guid id, ISender sender, CancellationToken ct)
    {
        await sender.Send(new DeleteIssueCommand(id), ct);
        return TypedResults.NoContent();
    }
}
