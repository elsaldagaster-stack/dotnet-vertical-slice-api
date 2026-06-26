using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.UpdateIssue;

public static class UpdateIssueEndpoint
{
    public static RouteGroupBuilder MapUpdateIssue(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", Handle).WithName("UpdateIssue")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    private static async Task<IResult> Handle(
        Guid id, [FromBody] UpdateIssueRequest body, ISender sender, CancellationToken ct)
    {
        await sender.Send(new UpdateIssueCommand(id, body.Title, body.Description, body.Priority, body.Type), ct);
        return TypedResults.NoContent();
    }

    private record UpdateIssueRequest(string Title, string? Description, IssuePriority Priority, IssueType Type);
}
