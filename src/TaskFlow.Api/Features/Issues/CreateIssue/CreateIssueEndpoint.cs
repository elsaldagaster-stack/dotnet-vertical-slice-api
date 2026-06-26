using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.CreateIssue;

public static class CreateIssueEndpoint
{
    public static RouteGroupBuilder MapCreateIssue(this RouteGroupBuilder group)
    {
        group.MapPost("/", Handle)
            .WithName("CreateIssue")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        return group;
    }

    private static async Task<IResult> Handle(
        Guid projectId,
        [FromBody] CreateIssueRequest body,
        ISender sender, CancellationToken ct)
    {
        var id = await sender.Send(
            new CreateIssueCommand(projectId, body.Title, body.Description,
                body.Priority, body.Type, body.ReporterId), ct);
        return TypedResults.Created($"/api/issues/{id}", id);
    }

    private record CreateIssueRequest(
        string Title, string? Description,
        IssuePriority Priority, IssueType Type, Guid ReporterId);
}
