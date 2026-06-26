using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.TransitionIssueStatus;

public static class TransitionIssueStatusEndpoint
{
    public static RouteGroupBuilder MapTransitionIssueStatus(this RouteGroupBuilder group)
    {
        group.MapPatch("/{id:guid}/status", Handle).WithName("TransitionIssueStatus")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        return group;
    }

    private static async Task<IResult> Handle(
        Guid id, [FromBody] TransitionRequest body, ISender sender, CancellationToken ct)
    {
        await sender.Send(new TransitionIssueStatusCommand(id, body.TargetStatus), ct);
        return TypedResults.NoContent();
    }

    private record TransitionRequest(IssueStatus TargetStatus);
}
