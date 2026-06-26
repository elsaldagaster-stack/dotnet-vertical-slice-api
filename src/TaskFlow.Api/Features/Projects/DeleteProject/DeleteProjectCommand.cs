using MediatR;

namespace TaskFlow.Api.Features.Projects.DeleteProject;

public record DeleteProjectCommand(Guid Id) : IRequest<Unit>;
