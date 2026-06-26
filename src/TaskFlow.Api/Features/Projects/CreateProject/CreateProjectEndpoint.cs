using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.Api.Features.Projects.CreateProject;

public static class CreateProjectEndpoint
{
    public static RouteGroupBuilder MapCreateProject(this RouteGroupBuilder group)
    {
        group.MapPost("/", Handle)
            .WithName("CreateProject")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
        return group;
    }

    private static async Task<IResult> Handle(
        [FromBody] CreateProjectCommand command, ISender sender, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return TypedResults.Created($"/api/projects/{id}", id);
    }
}
