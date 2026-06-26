using MediatR;

namespace TaskFlow.Api.Features.Issues.GetIssueById;

public static class GetIssueByIdEndpoint
{
    public static RouteGroupBuilder MapGetIssueById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetIssueById")
            .Produces<IssueDetailDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);
        return group;
    }

    private static async Task<IResult> Handle(Guid id, ISender sender, CancellationToken ct)
        => TypedResults.Ok(await sender.Send(new GetIssueByIdQuery(id), ct));
}
