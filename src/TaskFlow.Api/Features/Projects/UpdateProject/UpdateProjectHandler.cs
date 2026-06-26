using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Projects.UpdateProject;

public sealed class UpdateProjectHandler(AppDbContext db) : IRequestHandler<UpdateProjectCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Project '{request.Id}' not found.");

        project.Name = request.Name;
        project.Description = request.Description;
        project.UpdatedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
