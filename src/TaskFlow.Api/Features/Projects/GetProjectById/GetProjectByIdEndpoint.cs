using MediatR;

namespace TaskFlow.Api.Features.Projects.GetProjectById;

public static class GetProjectByIdEndpoint
{
    public static RouteGroupBuilder MapGetProjectById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetProjectById")
            .Produces<ProjectDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    private static async Task<IResult> Handle(Guid id, ISender sender, CancellationToken ct)
        => TypedResults.Ok(await sender.Send(new GetProjectByIdQuery(id), ct));
}
