using MediatR;

namespace TaskFlow.Api.Features.Projects.GetProjectById;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDetailDto>;

public record ProjectDetailDto(
    Guid Id, string Name, string? Description,
    DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
