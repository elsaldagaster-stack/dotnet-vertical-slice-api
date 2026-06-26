using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Projects.GetProjectById;

public sealed class GetProjectByIdHandler(AppDbContext db)
    : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto>
{
    public async Task<ProjectDetailDto> Handle(
        GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await db.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Project '{request.Id}' not found.");

        return new ProjectDetailDto(
            project.Id, project.Name, project.Description,
            project.CreatedAt, project.UpdatedAt);
    }
}
