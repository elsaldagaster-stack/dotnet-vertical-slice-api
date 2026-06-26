using MediatR;

namespace TaskFlow.Api.Features.Projects.CreateProject;

public record CreateProjectCommand(string Name, string? Description) : IRequest<Guid>;
