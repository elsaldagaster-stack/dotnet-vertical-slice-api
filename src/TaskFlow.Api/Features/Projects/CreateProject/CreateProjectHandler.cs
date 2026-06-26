using MediatR;
using TaskFlow.Api.Domain.Entities;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Projects.CreateProject;

public sealed class CreateProjectHandler(AppDbContext db) : IRequestHandler<CreateProjectCommand, Guid>
{
    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        db.Projects.Add(project);
        await db.SaveChangesAsync(cancellationToken);
        return project.Id;
    }
}
