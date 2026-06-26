using MediatR;

namespace TaskFlow.Api.Features.Projects.GetProjects;

public static class GetProjectsEndpoint
{
    public static RouteGroupBuilder MapGetProjects(this RouteGroupBuilder group)
    {
        group.MapGet("/", Handle)
            .WithName("GetProjects")
            .Produces<TaskFlow.Api.Infrastructure.Common.Pagination.CursorPage<ProjectSummaryDto>>();
        return group;
    }

    private static async Task<IResult> Handle(
        ISender sender, string? cursor, int limit = 20, string? search = null, CancellationToken ct = default)
        => TypedResults.Ok(await sender.Send(new GetProjectsQuery(cursor, limit, search), ct));
}
