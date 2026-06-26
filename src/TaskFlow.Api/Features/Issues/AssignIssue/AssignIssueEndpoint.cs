using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Features.Issues.AssignIssue;

public static class AssignIssueEndpoint
{
    public static RouteGroupBuilder MapAssignIssue(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}/assign", Handle).WithName("AssignIssue")
            .Produces(StatusCodes.Status204NoContent);
        return group;
    }

    private static async Task<IResult> Handle(
        Guid id, [FromBody] AssignRequest body, ISender sender, CancellationToken ct)
    {
        await sender.Send(new AssignIssueCommand(id, body.AssigneeId), ct);
        return TypedResults.NoContent();
    }

    private record AssignRequest(Guid? AssigneeId);
}
