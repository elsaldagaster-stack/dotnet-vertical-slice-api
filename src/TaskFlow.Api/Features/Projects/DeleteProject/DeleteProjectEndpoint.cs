using MediatR;

namespace TaskFlow.Api.Features.Projects.DeleteProject;

public static class DeleteProjectEndpoint
{
    public static RouteGroupBuilder MapDeleteProject(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", Handle)
            .WithName("DeleteProject")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    private static async Task<IResult> Handle(Guid id, ISender sender, CancellationToken ct)
    {
        await sender.Send(new DeleteProjectCommand(id), ct);
        return TypedResults.NoContent();
    }
}
