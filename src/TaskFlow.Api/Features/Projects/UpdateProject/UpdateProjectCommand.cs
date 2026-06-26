using MediatR;

namespace TaskFlow.Api.Features.Projects.UpdateProject;

public record UpdateProjectCommand(Guid Id, string Name, string? Description) : IRequest<Unit>;
