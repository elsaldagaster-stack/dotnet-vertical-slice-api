using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Projects.DeleteProject;

public sealed class DeleteProjectHandler(AppDbContext db) : IRequestHandler<DeleteProjectCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await db.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Project '{request.Id}' not found.");

        db.Projects.Remove(project);
        await db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
