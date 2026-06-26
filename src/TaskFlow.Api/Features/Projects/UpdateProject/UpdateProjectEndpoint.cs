using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Features.Projects.UpdateProject;

public static class UpdateProjectEndpoint
{
    public static RouteGroupBuilder MapUpdateProject(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", Handle)
            .WithName("UpdateProject")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        return group;
    }

    private static async Task<IResult> Handle(
        Guid id, [FromBody] UpdateProjectRequest body, ISender sender, CancellationToken ct)
    {
        await sender.Send(new UpdateProjectCommand(id, body.Name, body.Description), ct);
        return TypedResults.NoContent();
    }

    private record UpdateProjectRequest(string Name, string? Description);
}
