using MediatR;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.GetIssues;

public static class GetIssuesEndpoint
{
    public static RouteGroupBuilder MapGetIssues(this RouteGroupBuilder group)
    {
        group.MapGet("/", Handle).WithName("GetIssues");
        return group;
    }

    private static async Task<IResult> Handle(
        Guid projectId, ISender sender,
        string? cursor, int limit = 20,
        IssueStatus? status = null, IssuePriority? priority = null,
        Guid? assigneeId = null, CancellationToken ct = default)
        => TypedResults.Ok(await sender.Send(
            new GetIssuesQuery(projectId, cursor, limit, status, priority, assigneeId), ct));
}
